using AutoMapper;
using MediatR;
using Metflix.BL.MediatR.CommandHandlers.Movies;
using Metflix.BL.MediatR.QueryHandlers.Movies;
using Metflix.BL.Services.Contracts;
using Metflix.DL.Repositories.Contracts;
using Metflix.Host.AutoMapper;
using Metflix.Models.DbModels;
using Metflix.Models.Mediatr.Commands.Movies;
using Metflix.Models.Mediatr.Queries.Movies;
using Metflix.Models.Requests.Movies;
using Metflix.Models.Responses;
using Metflix.Models.Responses.Movies;
using Metflix.Models.Responses.Movies.MovieDtos;
using Moq;
using Utils;

namespace Metflix.Tests.MediatRTests
{
    public class MovieHandlersTests
    {

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

        private readonly IEnumerable<Movie> _testMovies = new List<Movie>()
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
                AvailableQuantity = 14,
                TotalQuantity = 15,
                LastChanged = new DateTime(2022, 11, 10),
                PricePerDay = 3m,
                Name = "TestMovie2",
                Year = 2010
            },
        };

        private readonly IMapper _mapper;
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IInventoryService> _inventoryServiceMock;
        public MovieHandlersTests()
        {
            var mockMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MovieProfile());
            });
            _mapper = mockMapperConfig.CreateMapper();
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _inventoryServiceMock = new Mock<IInventoryService>();
        }
        [Fact]
        public async Task AddInventory_ExistingMovie_SuccessfulPath()
        {
            //Arange
            var response = GenerateMovieResponse(HttpStatusCode.OK, null!, _testMovie);

            var request = new AddInventoryRequest()
            {
                MovieId = 1,
                Quantity = 1
            };

            var command = new AddInventoryCommand(request, "");

            _inventoryServiceMock.Setup(x => x.AdjustInventoryAndProduceToKafkaTopic(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            _movieRepositoryMock.Setup(x => x.GetById(command.Request.MovieId, It.IsAny<CancellationToken>())).ReturnsAsync(_testMovie);
            var handler = new AddInventoryCommandHandler(_movieRepositoryMock.Object, _inventoryServiceMock.Object);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode200Or201WithEmptyMessageResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task AddInventory_InvalidMovieId_NotFoundPath()
        {
            //Arange
            var response = GenerateMovieResponse(HttpStatusCode.NotFound, ResponseMessages.IdNotFound, default(Movie)!);

            var request = new AddInventoryRequest()
            {
                MovieId = 1,
                Quantity = 1
            };

            var command = new AddInventoryCommand(request, "");


            _movieRepositoryMock.Setup(x => x.GetById(command.Request.MovieId, It.IsAny<CancellationToken>())).ReturnsAsync(default(Movie));
            var handler = new AddInventoryCommandHandler(_movieRepositoryMock.Object, _inventoryServiceMock.Object);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert
            AssertStatusCode404WithEmptyModelResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task RemoveInventory_ExistingMovie_SuccessfulPath()
        {
            //Arange
            var response = GenerateMovieResponse(HttpStatusCode.OK, null!, _testMovie);

            var request = new RemoveInventoryRequest()
            {
                MovieId = 1,
                Quantity = 1
            };

            var command = new RemoveInventoryCommand(request, "");

            _inventoryServiceMock.Setup(x => x.AdjustInventoryAndProduceToKafkaTopic(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            _movieRepositoryMock.Setup(x => x.GetById(command.Request.MovieId, It.IsAny<CancellationToken>())).ReturnsAsync(_testMovie);
            var handler = new RemoveInventoryCommandHandler(_movieRepositoryMock.Object, _inventoryServiceMock.Object);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode200Or201WithEmptyMessageResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task RemoveInventory_InvalidMovieId_NotFoundPath()
        {
            //Arange            
            var response = GenerateMovieResponse(HttpStatusCode.NotFound, ResponseMessages.IdNotFound, default(Movie)!);

            var request = new RemoveInventoryRequest()
            {
                MovieId = 1,
                Quantity = 1
            };

            var command = new RemoveInventoryCommand(request, "");


            _movieRepositoryMock.Setup(x => x.GetById(command.Request.MovieId, It.IsAny<CancellationToken>())).ReturnsAsync(default(Movie));
            var handler = new RemoveInventoryCommandHandler(_movieRepositoryMock.Object, _inventoryServiceMock.Object);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode404WithEmptyModelResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task AddMovie_SuccessfulPath()
        {
            //Arange
            var response = GenerateMovieResponse(HttpStatusCode.Created, null!, _testMovie);

            var request = new AddMovieRequest()
            {
                Name = "Movie",
                PricePerDay = 3,
                TotalQuantity = 5,
                Year = 2000
            };

            var command = new AddMovieCommand(request);

            _movieRepositoryMock.Setup(x => x.Add(It.IsAny<Movie>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testMovie);
            var handler = new AddMovieCommandHandler(_movieRepositoryMock.Object, _mapper);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode200Or201WithEmptyMessageResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task DeleteMovie_ExistingMovie_SuccessfulPath()
        {
            //Arange

            var response = GenerateMovieResponse(HttpStatusCode.NoContent, null!, default(Movie)!);

            var request = new ByIntIdRequest()
            {
                Id = 1
            };

            var command = new DeleteMovieCommand(request);

            _movieRepositoryMock.Setup(x => x.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            var handler = new DeleteMovieCommandHandler(_movieRepositoryMock.Object);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode204WithEmptyModelAndMessageResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task DeleteMovie_InvalidMovieId_NotFoundPath()
        {
            //Arange

            var response = GenerateMovieResponse(HttpStatusCode.NotFound, ResponseMessages.IdNotFound, default(Movie)!);

            var request = new ByIntIdRequest()
            {
                Id = 1
            };

            var command = new DeleteMovieCommand(request);

            _movieRepositoryMock.Setup(x => x.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            var handler = new DeleteMovieCommandHandler(_movieRepositoryMock.Object);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode404WithEmptyModelResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task UpdateMovie_ExistingMovie_SuccessfulPath()
        {
            //Arange

            var response = GenerateMovieResponse(HttpStatusCode.OK, null!, _testMovie);

            var request = new UpdateMovieRequest()
            {
                Name = "Movie",
                PricePerDay = 3,
                TotalQuantity = 5,
                Year = 2000,
                Id = 1,

            };

            var command = new UpdateMovieCommand(request);

            _movieRepositoryMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testMovie);
            _movieRepositoryMock.Setup(x => x.Update(It.IsAny<Movie>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testMovie);
            var handler = new UpdateMovieCommandHandler(_movieRepositoryMock.Object, _mapper);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode200Or201WithEmptyMessageResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task UpdateMovie_InvalidMovie_NotFoundPath()
        {
            //Arange
            var response = GenerateMovieResponse(HttpStatusCode.NotFound, ResponseMessages.IdNotFound, default(Movie)!);

            var request = new UpdateMovieRequest()
            {
                Name = "Movie",
                PricePerDay = 3,
                TotalQuantity = 5,
                Year = 2000,
                Id = 1,

            };

            var command = new UpdateMovieCommand(request);

            _movieRepositoryMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Movie));

            var handler = new UpdateMovieCommandHandler(_movieRepositoryMock.Object, _mapper);

            //Act
            var handlerResponse = await handler.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            AssertStatusCode404WithEmptyModelResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task GetAllMovies_SuccessfulPath()
        {
            //Arange
            var response = new MovieCollectionResponse()
            {
                Model = _mapper.Map<IEnumerable<MovieDto>>(_testMovies),
                HttpStatusCode = HttpStatusCode.OK,
            };          

            var query = new GetAllMoviesQuery();

            _movieRepositoryMock.Setup(x => x.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(_testMovies);

            var handler = new GetAllMoviesQueryHandler(_movieRepositoryMock.Object, _mapper);

            //Act
            var handlerResponse = await handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            AssertStatusCode200Or201WithEmptyMessageResponseEquality(response, handlerResponse);           
        }

        [Fact]
        public async Task GetAvailableMovies_SuccessfulPath()
        {
            //Arange
            var response = new AvailableMoviesResponse()
            {
                Model = _mapper.Map<IEnumerable<AvailableMovieDto>>(_testMovies),
                HttpStatusCode = HttpStatusCode.OK,
            };

            var query = new GetAvailableMoviesQuery();

            _movieRepositoryMock.Setup(x => x.GetAllAvailableMovies(It.IsAny<CancellationToken>())).ReturnsAsync(_testMovies);

            var handler = new GetAvailableMoviesQueryHandler(_movieRepositoryMock.Object, _mapper);

            //Act
            var handlerResponse = await handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            AssertStatusCode200Or201WithEmptyMessageResponseEquality(response, handlerResponse);
        }       

        [Fact]
        public async Task GetMovieById_ExistingMovie_SuccessfulPath()
        {
            //Arange

            var response = GenerateMovieResponse(HttpStatusCode.OK, null!, _testMovie);

            var request = new ByIntIdRequest()
            {
                Id = 1
            };

            var query = new GetMovieByIdQuery(request);

            _movieRepositoryMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testMovie);
            var handler = new GetMovieByIdQueryHandler(_movieRepositoryMock.Object, _mapper);

            //Act
            var handlerResponse = await handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            AssertStatusCode200Or201WithEmptyMessageResponseEquality(response, handlerResponse);
        }

        [Fact]
        public async Task GetMovieById_InvalidMovie_NotFoundPath()
        {
            //Arange

            var response = GenerateMovieResponse(HttpStatusCode.NotFound, ResponseMessages.IdNotFound, default(Movie)!);

            var request = new ByIntIdRequest()
            {
                Id = 1
            };

            var query = new GetMovieByIdQuery(request);

            _movieRepositoryMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Movie)!);
            var handler = new GetMovieByIdQueryHandler(_movieRepositoryMock.Object, _mapper);

            //Act
            var handlerResponse = await handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            AssertStatusCode404WithEmptyModelResponseEquality(response, handlerResponse);
        }

        private MovieResponse GenerateMovieResponse(HttpStatusCode statusCode, string message, Movie movie)
        {
            var model = movie == default(Movie) ? null : _mapper.Map<MovieDto>(movie);

            return new MovieResponse()
            {
                HttpStatusCode = statusCode,
                Message = message,
                Model = model
            };
        }
    }
}