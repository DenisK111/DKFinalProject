using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Metflix.DL.Repositories.Contracts;
using Metflix.Host.AutoMapper;
using Metflix.Kafka.Producers;
using Metflix.Models.Configurations.KafkaSettings.Producers;
using Metflix.Models.DbModels;
using Metflix.Models.KafkaModels;
using Metflix.Models.Responses.Movies.MovieDtos;
using Metflix.Models.Responses.Movies;
using Moq;
using Metflix.Models.Responses.Purchases;
using Metflix.Models.Responses.Purchases.PurchaseDtos;

namespace Metflix.Tests.MediatRTests
{
    public class PurchaseHandlersTests
    {
        private readonly Mock<IUserMovieRepository> _userMovieRepositoryMock;
        private readonly Mock<IPurchaseRepository> _purchaseRepositoryMock;
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<ITempPurchaseDataRepository> _tempPurchaseRepositoryMock;
        private readonly Mock<GenericProducer<string, PurchaseUserInputData, KafkaUserPurchaseInputProducerSettings>> _kafkaProducerMock;
        private readonly IMapper _mapper;
        public PurchaseHandlersTests()
        {
            _userMovieRepositoryMock = new Mock<IUserMovieRepository>();
            _purchaseRepositoryMock = new Mock<IPurchaseRepository>();
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _tempPurchaseRepositoryMock = new Mock<ITempPurchaseDataRepository>();
            _kafkaProducerMock = new Mock<GenericProducer<string, PurchaseUserInputData, KafkaUserPurchaseInputProducerSettings>>();
            var mockMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PurchaseProfile());
                cfg.AddProfile(new MovieProfile());
            });
            _mapper = mockMapperConfig.CreateMapper();
        }

        private PurchaseResponse GeneratePurchaseResponse<T>(HttpStatusCode statusCode, string message, Purchase purchase)
        {
            var model = purchase == default(Purchase) ? null : _mapper.Map<PurchaseDto>(purchase);

            return new PurchaseResponse()
            {
                HttpStatusCode = statusCode,
                Message = message,
                Model = model
            };
        }
    }
}
