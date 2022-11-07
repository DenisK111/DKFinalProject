using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Metflix.BL.TempData;
using Metflix.DL.Repositories.Contracts;
using Metflix.Kafka.Producers;
using Metflix.Models.Configurations.KafkaSettings.Producers;
using Metflix.Models.DbModels;
using Metflix.Models.KafkaModels;
using Metflix.Models.Mediatr.Commands.Purchases;
using Metflix.Models.Responses.Purchases;
using Metflix.Models.Responses.Purchases.PurchaseDtos;
using Utils;

namespace Metflix.BL.MediatR.CommandHandlers.Purchases
{
    public class MakePurchaseCommandHandler : IRequestHandler<MakePurchaseCommand, PurchaseResponse>
    {
        private readonly IMapper _mapper;       
        private readonly IMovieRepository _movieRepository;
        private readonly GenericProducer<string, PurchaseUserInputData, KafkaUserPurchaseInputProducerSettings> _producer;

        public MakePurchaseCommandHandler(IMapper mapper, IMovieRepository movieRepository, GenericProducer<string, PurchaseUserInputData, KafkaUserPurchaseInputProducerSettings> producer)
        {
            _mapper = mapper;
         
            _movieRepository = movieRepository;
            _producer = producer;
        }

        public async Task<PurchaseResponse> Handle(MakePurchaseCommand request, CancellationToken cancellationToken)
        {
            var invalidMovieIds = new List<int>();
            var notEnoughQuantity = new List<int>();

            foreach (var id in request.Request.MovieIds)
            {
                var movie = await _movieRepository.GetById(id, cancellationToken);
                if (movie == null)
                {
                    invalidMovieIds.Add(id);
                    continue;
                }

                if (!invalidMovieIds.Any() && movie.AvailableQuantity < 1)
                {
                    notEnoughQuantity.Add(id);
                    continue;
                }
            }

            if (invalidMovieIds.Any())
            {
                return new PurchaseResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = string.Format(ResponseMessages.InvalidMovieIds, string.Join(", ", invalidMovieIds))
                };
            }

            if (notEnoughQuantity.Any())
            {
                return new PurchaseResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = string.Format(ResponseMessages.NotEnoughQtyMovies, string.Join(", ", notEnoughQuantity))
                };
            }

            var kafkaMessageValue = new PurchaseUserInputData()
            {
                Id = request.UserId,
                MovieIds = request.Request.MovieIds,
                Days = request.Request.Days,
            };

            if (PurchaseTempData.PendingPurchases.ContainsKey(request.UserId))
            {
                return new PurchaseResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessages.PendingPurchase,
                };
            }

            PurchaseTempData.PendingPurchases.TryAdd(request.UserId, null);

            await _producer.ProduceAsync(kafkaMessageValue.GetKey(), kafkaMessageValue);

            while (PurchaseTempData.PendingPurchases[request.UserId] is null && !cancellationToken.IsCancellationRequested)
            { }

            PurchaseResponse purchaseResponse = null!;
            if (PurchaseTempData.PendingPurchases[request.UserId] != null)
            {
                purchaseResponse = new PurchaseResponse()
                {
                    Model = _mapper.Map<PurchaseDto>(PurchaseTempData.PendingPurchases[request.UserId]),
                    HttpStatusCode = HttpStatusCode.Created,
                };
            }

            else
            {
                purchaseResponse = new PurchaseResponse()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Message = "Something went wrong."
                };
            }

            PurchaseTempData.PendingPurchases.TryRemove(request.UserId, out Purchase? value);

            return purchaseResponse;
        }
    }
}
