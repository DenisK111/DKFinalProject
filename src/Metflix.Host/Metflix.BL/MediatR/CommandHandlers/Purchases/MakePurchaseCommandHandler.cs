using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.DbModels;
using Metflix.Models.Mediatr.Commands.Purchases;
using Metflix.Models.Responses.Purchases;
using Metflix.Models.Responses.Purchases.PurchaseDtos;
using Utils;

namespace Metflix.BL.MediatR.CommandHandlers.Purchases
{
    public class MakePurchaseCommandHandler : IRequestHandler<MakePurchaseCommand, PurchaseResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserMovieRepository _userMovieRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMovieRepository _movieRepository;

        public MakePurchaseCommandHandler(IMapper mapper, IUserMovieRepository userMovieRepository, IPurchaseRepository purchaseRepository, IMovieRepository movieRepository)
        {
            _mapper = mapper;
            _userMovieRepository = userMovieRepository;
            _purchaseRepository = purchaseRepository;
            _movieRepository = movieRepository;
        }

        public async Task<PurchaseResponse> Handle(MakePurchaseCommand request, CancellationToken cancellationToken)
        {
            var movies = new List<MovieRecord>();
            var invalidMovieIds = new List<int>();

            foreach (var id in request.Request.MovieIds)
            {
                var movie = await _movieRepository.GetById(id,cancellationToken);
                if (movie == null)
                {
                    invalidMovieIds.Add(id);
                    continue;
                }
                var movieRecord = _mapper.Map<MovieRecord>(movie);
                movies.Add(movieRecord);
            }

            if (invalidMovieIds.Any())
            {
                return new PurchaseResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = string.Format(ResponseMessages.InvalidMovieIds, string.Join(", ", invalidMovieIds))
                };
            }

            foreach (var movieId in request.Request.MovieIds)
            {
                await _userMovieRepository.Add(new UserMovie()
                {
                    UserId=request.UserId,
                    MovieId=movieId,
                    DaysFor=request.Request.Days,                    
                });
            }

            var purchase = new Purchase()
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Movies = movies,
                LastChanged = DateTime.UtcNow,
                Days = request.Request.Days,
                DueDate = DateTime.UtcNow.AddDays(request.Request.Days),
                PurchaseDate = DateTime.UtcNow,
                TotalPrice = movies.Sum(x => x.PricePerDay) * request.Request.Days,
            };

            await _purchaseRepository.AddPurchase(purchase, cancellationToken);
            var purchaseDto = _mapper.Map<PurchaseDto>(purchase);

            return new PurchaseResponse()
            {
                HttpStatusCode = HttpStatusCode.Created,
                Model = purchaseDto,
            };
        }
    }
}
