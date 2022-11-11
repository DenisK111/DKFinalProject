using AutoMapper;
using Metflix.DL.Repositories.Contracts;
using Metflix.Host.AutoMapper;
using Metflix.Models.Configurations.KafkaSettings.Producers;
using Metflix.Models.DbModels;
using Metflix.Models.KafkaModels;
using Metflix.Models.Responses.Movies.MovieDtos;
using Moq;
using Metflix.Models.Responses.Purchases;
using Metflix.Models.Responses.Purchases.PurchaseDtos;
using MessagePack;
using Metflix.Models.Mediatr.Commands.Purchases;
using Metflix.Models.Requests.Purchase;
using Metflix.BL.MediatR.CommandHandlers.Purchases;
using Metflix.Kafka.Contracts;
using Metflix.BL.MediatR.QueryHandlers.Purchases;
using Metflix.Models.Mediatr.Queries.Purchases;
using Metflix.Models.DbModels.DbDtos;
using Metflix.Models.Common;

namespace Metflix.Tests.MediatRTests
{
    public class PurchaseHandlersTests
    {
        private readonly UserMovie _testUserMovieNotReturned = new UserMovie()
        {
            CreatedOn = DateTime.UtcNow,
            Id = 1,
            MovieId = 1,
            UserId = "Test",
            DaysFor = 2,
            DueDate = DateTime.UtcNow.AddDays(2),
            IsReturned = false,
            LastChanged = DateTime.UtcNow,

        };

        private readonly UserMovie _testUserMovieReturned = new UserMovie()
        {
            CreatedOn = DateTime.UtcNow,
            Id = 1,
            MovieId = 1,
            UserId = "Test",
            DaysFor = 2,
            DueDate = DateTime.UtcNow.AddDays(2),
            IsReturned = true,
            LastChanged = DateTime.UtcNow,

        };

        private readonly Purchase _testPurchase = new Purchase()
        {
            Id = Guid.NewGuid(),
            Movies = new List<MovieRecord>()
            {
                new MovieRecord()
                {
                    Id = 1,
                    Name = "My Movie",
                    PricePerDay = 10
                }
            },
            UserMovieIds = new List<int>()
            {
                1
            },
            Days = 3,
            PurchaseDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(3),
            LastChanged = DateTime.UtcNow,
            TotalPrice = 30,
            UserId = Guid.NewGuid().ToString()
        };

        private readonly IEnumerable<Purchase> _testPurchases = new List<Purchase>()
        {
            new Purchase()
        {
            Id = Guid.NewGuid(),
            Movies = new List<MovieRecord>()
            {
                new MovieRecord()
                {
                    Id = 1,
                    Name = "My Movie",
                    PricePerDay = 10
                }
            },
            UserMovieIds = new List<int>()
            {
                1
            },
            Days = 3,
            PurchaseDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(3),
            LastChanged = DateTime.UtcNow,
            TotalPrice = 30,
            UserId = Guid.NewGuid().ToString()
        },
            new Purchase()
        {
            Id = Guid.NewGuid(),
            Movies = new List<MovieRecord>()
            {
                new MovieRecord()
                {
                    Id = 2,
                    Name = "My Movie2",
                    PricePerDay = 5
                }
            },
            UserMovieIds = new List<int>()
            {
                1
            },
            Days = 2,
            PurchaseDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(2),
            LastChanged = DateTime.UtcNow,
            TotalPrice = 10,
            UserId = Guid.NewGuid().ToString()
            },
        };
        private readonly Movie _testMovie = new Movie()
        {
            Id = 1,
            AvailableQuantity = 10,
            TotalQuantity = 10,
            LastChanged = new DateTime(2022, 10, 10),
            PricePerDay = 2m,
            Name = "TestMovie1",
            Year = 2000
        };

        private readonly List<Movie> _testMovies = new List<Movie>()
        {
            new Movie()
            {
                Id = 1,
                AvailableQuantity = 10,
                TotalQuantity = 10,
                LastChanged = new DateTime(2022, 10, 10),
                PricePerDay = 2m,
                Name = "TestMovie1",
                Year = 2000
            },
            new Movie()
            {
                Id = 2,
                AvailableQuantity = 0,
                TotalQuantity = 15,
                LastChanged = new DateTime(2022, 11, 10),
                PricePerDay = 3m,
                Name = "TestMovie2",
                Year = 2010
            },
        };

        private readonly Mock<IUserMovieRepository> _userMovieRepositoryMock;
        private readonly Mock<IPurchaseRepository> _purchaseRepositoryMock;
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<ITempPurchaseDataRepository> _tempPurchaseRepositoryMock;
        private readonly Mock<IGenericProducer<string, PurchaseUserInputData, KafkaUserPurchaseInputProducerSettings>> _kafkaProducerMock;
        private readonly IMapper _mapper;
        public PurchaseHandlersTests()
        {
            _userMovieRepositoryMock = new Mock<IUserMovieRepository>();
            _purchaseRepositoryMock = new Mock<IPurchaseRepository>();
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _tempPurchaseRepositoryMock = new Mock<ITempPurchaseDataRepository>();
            _kafkaProducerMock = new Mock<IGenericProducer<string, PurchaseUserInputData, KafkaUserPurchaseInputProducerSettings>>();
            var mockMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PurchaseProfile());
                cfg.AddProfile(new MovieProfile());
            });
            _mapper = mockMapperConfig.CreateMapper();
        }

        [Fact]
        public async Task MakePurchase_SuccessfulPath()
        {
            //Arange
            var response = GenerateResponse<PurchaseResponse, Purchase, PurchaseDto>(HttpStatusCode.Created, null!, _testPurchase);

            var request = new PurchaseRequest()
            {
                MovieIds = new[] { 1 },
                Days = 3
            };

            var command = new MakePurchaseCommand(request, _testPurchase.UserId);

            _kafkaProducerMock.Setup(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<PurchaseUserInputData>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _movieRepositoryMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testMovie);
            _tempPurchaseRepositoryMock.Setup(x => x.ContainsKeyAsync(It.IsAny<string>())).ReturnsAsync(false);
            _tempPurchaseRepositoryMock.Setup(x => x.GetValueAsync(It.IsAny<string>())).ReturnsAsync(MessagePackSerializer.Serialize(_testPurchase));
            var handler = new MakePurchaseCommandHandler(_mapper, _movieRepositoryMock.Object, _kafkaProducerMock.Object, _tempPurchaseRepositoryMock.Object);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode200Or201WithEmptyMessageResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task MakePurchase_IvalidMovieIds_BadRequestPath()
        {
            //Arange
            var movieIds = new[] { 1, 10, 20 };
            var invalidIds = new[] { 10, 20 };

            var data = new List<Movie?>()
            {
                _testMovie,null,null
             };
            var response = GenerateResponse<PurchaseResponse, Purchase, PurchaseDto>(HttpStatusCode.BadRequest, string.Format(ResponseMessages.InvalidMovieIds, string.Join(", ", invalidIds)), default(Purchase)!);

            var request = new PurchaseRequest()
            {
                MovieIds = new[] { 1, 10, 20 },
                Days = 3
            };

            var command = new MakePurchaseCommand(request, _testPurchase.UserId);

            _movieRepositoryMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Movie));
            _movieRepositoryMock.Setup(x => x.GetById(It.Is<int>(i => i == 1), It.IsAny<CancellationToken>())).ReturnsAsync(_testMovie);

            var handler = new MakePurchaseCommandHandler(_mapper, _movieRepositoryMock.Object, _kafkaProducerMock.Object, _tempPurchaseRepositoryMock.Object);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode404WithEmptyModelResponseEquality(response, handlerResponse);
        }


        [Fact]
        public async Task MakePurchase_NotEnoughQuantity_BadRequestPath()
        {
            //Arange
            var movieIds = new[] { 1, 2 };
            var notEnoughQty = new[] { 2 };

            var data = new List<Movie?>()
            {
                _testMovie,null,null
             };
            var response = GenerateResponse<PurchaseResponse, Purchase, PurchaseDto>(HttpStatusCode.BadRequest, string.Format(ResponseMessages.NotEnoughQtyMovies, string.Join(", ", notEnoughQty)), default(Purchase)!);

            var request = new PurchaseRequest()
            {
                MovieIds = new[] { 1, 2 },
                Days = 3
            };

            var command = new MakePurchaseCommand(request, _testPurchase.UserId);

            _movieRepositoryMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testMovies[1]);
            _movieRepositoryMock.Setup(x => x.GetById(It.Is<int>(i => i == 1), It.IsAny<CancellationToken>())).ReturnsAsync(_testMovie);

            var handler = new MakePurchaseCommandHandler(_mapper, _movieRepositoryMock.Object, _kafkaProducerMock.Object, _tempPurchaseRepositoryMock.Object);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode404WithEmptyModelResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task MakePurchase_PendingPurchase_BadRequestPath()
        {
            //Arange

            var response = GenerateResponse<PurchaseResponse, Purchase, PurchaseDto>(HttpStatusCode.BadRequest, ResponseMessages.PendingPurchase, default(Purchase)!);

            var request = new PurchaseRequest()
            {
                MovieIds = new[] { 1, 10, 20 },
                Days = 3
            };

            var command = new MakePurchaseCommand(request, _testPurchase.UserId);

            _movieRepositoryMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testMovie);
            _tempPurchaseRepositoryMock.Setup(x => x.ContainsKeyAsync(It.IsAny<string>())).ReturnsAsync(true);
            var handler = new MakePurchaseCommandHandler(_mapper, _movieRepositoryMock.Object, _kafkaProducerMock.Object, _tempPurchaseRepositoryMock.Object);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode404WithEmptyModelResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task MakePurchase_NoReturnedValue_InternalServerErrorPath()
        {
            //Arange
            var response = GenerateResponse<PurchaseResponse, Purchase, PurchaseDto>(HttpStatusCode.InternalServerError, ResponseMessages.InternalServerErrorMessage, default(Purchase)!);

            var request = new PurchaseRequest()
            {
                MovieIds = new[] { 1 },
                Days = 3
            };

            var command = new MakePurchaseCommand(request, _testPurchase.UserId);

            _kafkaProducerMock.Setup(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<PurchaseUserInputData>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _movieRepositoryMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testMovie);
            _tempPurchaseRepositoryMock.Setup(x => x.ContainsKeyAsync(It.IsAny<string>())).ReturnsAsync(false);
            _tempPurchaseRepositoryMock.Setup(x => x.GetValueAsync(It.IsAny<string>())).ReturnsAsync(Array.Empty<byte>());
            var handler = new MakePurchaseCommandHandler(_mapper, _movieRepositoryMock.Object, _kafkaProducerMock.Object, _tempPurchaseRepositoryMock.Object);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode500WithEmptyModelResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task ReturnMovie_SuccessfulPath()
        {
            //Arange
            var response = GenerateResponse<ReturnMovieResponse, Movie, MovieDto>(HttpStatusCode.OK, null!, _testMovie);

            var request = new ReturnMovieRequest()
            {
                UserMovieId = 1
            };

            var command = new ReturnMovieCommand(request, "TestId");

            _userMovieRepositoryMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testUserMovieNotReturned);
            _movieRepositoryMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testMovie);
            var handler = new ReturnMovieCommandHandler(_userMovieRepositoryMock.Object, _movieRepositoryMock.Object, _mapper);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode200Or201WithEmptyMessageResponseEquality(response, handlerResponse);

        }

        [Fact]
        public async Task ReturnMovie_MovieAlreadyReturned_BadRequestPath()
        {
            //Arange
            var response = GenerateResponse<ReturnMovieResponse, Movie, MovieDto>(HttpStatusCode.BadRequest, ResponseMessages.MovieAlreadyReturned, default(Movie)!);

            var request = new ReturnMovieRequest()
            {
                UserMovieId = 1
            };

            var command = new ReturnMovieCommand(request, "TestId");

            _userMovieRepositoryMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testUserMovieReturned);

            var handler = new ReturnMovieCommandHandler(_userMovieRepositoryMock.Object, _movieRepositoryMock.Object, _mapper);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode400WithEmptyModelResponseEquality(response, handlerResponse);

        }

        [Fact]
        public async Task ReturnMovie_InvalidMovie_NotFoundPath()
        {
            //Arange
            var response = GenerateResponse<ReturnMovieResponse, Movie, MovieDto>(HttpStatusCode.NotFound, ResponseMessages.IdNotFound, default(Movie)!);

            var request = new ReturnMovieRequest()
            {
                UserMovieId = 1
            };

            var command = new ReturnMovieCommand(request, "TestId");

            _userMovieRepositoryMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(UserMovie));

            var handler = new ReturnMovieCommandHandler(_userMovieRepositoryMock.Object, _movieRepositoryMock.Object, _mapper);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode404WithEmptyModelResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task GetCurrentlyTakenMovies_SuccessfulPath()
        {
            //Arange
            var model = new List<UserMovieDbDto>()
                {
                    new UserMovieDbDto()
                    {
                        Id = 1,
                        DueDate = DateTime.UtcNow,
                        Name = "TestMovie",
                    }
            };

            var response = GenerateResponse<GetCurrentlyTakenMoviesResponse, IEnumerable<UserMovieDbDto>, IEnumerable<UserMovieDto>>(HttpStatusCode.OK, null!,model);                 

            var query = new GetCurrentlyTakenMoviesQuery("TestId");

            _userMovieRepositoryMock.Setup(x => x.GetAllUnreturnedByUserId(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(model);

            var handler = new GetCurrentlyTakenMoviesQueryHandler(_userMovieRepositoryMock.Object,_mapper);

            //Act
            var handlerResponse = await handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode200Or201WithEmptyMessageResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task GetMyPurchases_SuccessfulPath()
        {
            //Arange
            
            var response = GenerateResponse<PurchaseCollectionResponse, IEnumerable<Purchase>, IEnumerable<PurchaseDto>>(HttpStatusCode.OK, null!, _testPurchases);          
           
            var query = new GetMyPurchasesQuery("TestId");

            _purchaseRepositoryMock.Setup(x => x.GetAllByUserId(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testPurchases);

            var handler = new GetMyPurchasesQueryHandler(_purchaseRepositoryMock.Object,_mapper);

            //Act
            var handlerResponse = await handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode200Or201WithEmptyMessageResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task GetPurchaseByIdAndUserId_SuccessfulPath()
        {
            //Arange
            
            var response = GenerateResponse<PurchaseResponse,Purchase,PurchaseDto>(HttpStatusCode.OK, null!, _testPurchase);

            var query = new GetPurchaseByIdAndUserIdQuery(Guid.NewGuid(),"TestId");

            _purchaseRepositoryMock.Setup(x => x.GetByIdAndUserId(It.IsAny<Guid>(),It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testPurchase);

            var handler = new GetPurchaseByIdAndUserIdQueryHandler(_purchaseRepositoryMock.Object, _mapper);

            //Act
            var handlerResponse = await handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode200Or201WithEmptyMessageResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task GetPurchaseByIdAndUserId_NoMatches_NotFoundPath()
        {
            //Arange

            var response = GenerateResponse<PurchaseResponse, Purchase, PurchaseDto>(HttpStatusCode.NotFound, ResponseMessages.NoIdForUser, default(Purchase)!);

            var query = new GetPurchaseByIdAndUserIdQuery(Guid.NewGuid(), "TestId");

            _purchaseRepositoryMock.Setup(x => x.GetByIdAndUserId(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Purchase)!);

            var handler = new GetPurchaseByIdAndUserIdQueryHandler(_purchaseRepositoryMock.Object, _mapper);

            //Act
            var handlerResponse = await handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode404WithEmptyModelResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task GetPurchaseById_SuccessfulPath()
        {
            //Arange

            var response = GenerateResponse<PurchaseResponse, Purchase, PurchaseDto>(HttpStatusCode.OK, null!, _testPurchase);

            var query = new GetPurchaseByIdQuery(Guid.NewGuid());

            _purchaseRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testPurchase);

            var handler = new GetPurchaseByIdQueryHandler(_purchaseRepositoryMock.Object, _mapper);

            //Act
            var handlerResponse = await handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode200Or201WithEmptyMessageResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task GetPurchaseById_InvalidId_NotFoundPath()
        {
            //Arange

            var response = GenerateResponse<PurchaseResponse, Purchase, PurchaseDto>(HttpStatusCode.NotFound, ResponseMessages.IdNotFound, default(Purchase)!);

            var query = new GetPurchaseByIdQuery(Guid.NewGuid());

            _purchaseRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>(),It.IsAny<CancellationToken>())).ReturnsAsync(default(Purchase)!);

            var handler = new GetPurchaseByIdQueryHandler(_purchaseRepositoryMock.Object, _mapper);

            //Act
            var handlerResponse = await handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode404WithEmptyModelResponseEquality(response, handlerResponse);
        }      
    }
}
