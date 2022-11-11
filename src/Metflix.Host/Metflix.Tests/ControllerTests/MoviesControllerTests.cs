using FluentAssertions;
using MediatR;
using Metflix.Host.Controllers;
using Metflix.Models.DbModels;
using Metflix.Models.Mediatr.Commands.Movies;
using Metflix.Models.Mediatr.Queries.Movies;
using Metflix.Models.Requests.Movies;
using Metflix.Models.Responses.Movies;
using Metflix.Models.Responses.Movies.MovieDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Metflix.Tests.ControllerTests
{
    public class MoviesControllerTests
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

        private readonly Mock<IMediator> _mediator;
        private readonly MoviesController _controller;
        public MoviesControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _controller = new MoviesController(_mediator.Object);
        }

        [Fact]
        public async Task Movies_Add_SuccessfullPath()
        {
            //Arrange

            var mediatorResponse = GenerateResponse<MovieResponse, Movie, MovieDto>(HttpStatusCode.Created, null!, _testMovie);
            _mediator.Setup(x => x.Send(It.IsAny<AddMovieCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResponse);

            var request = new AddMovieRequest()
            {
                Name = _testMovie.Name,
                PricePerDay = _testMovie.PricePerDay,
                Year = _testMovie.Year,
                TotalQuantity = _testMovie.TotalQuantity,
            };
            var expectedActionName = nameof(MoviesController.GetById);
            var expectedRouteValues = new RouteValueDictionary()
            {
                ["Id"] = mediatorResponse.Model?.Id
            };

            //Act

            var result = await _controller.Add(request, new CancellationToken());
            var createdAtActionObjectResult = result as CreatedAtActionResult;
            var resultValue = createdAtActionObjectResult!.Value as MovieDto;
            var resultActionName = createdAtActionObjectResult!.ActionName;
            var actualRouteValues = createdAtActionObjectResult!.RouteValues;

            //Assert

            Assert.NotNull(createdAtActionObjectResult);
            Assert.NotNull(resultValue);
            mediatorResponse.Model.Should().BeEquivalentTo(resultValue);
            Assert.Equal(expectedActionName, resultActionName);
            Assert.Equal(expectedRouteValues, actualRouteValues);
        }

        [Fact]
        public async Task Movies_Update_SuccessfullPath()
        {
            //Arrange

            var mediatorResponse = GenerateResponse<MovieResponse, Movie, MovieDto>(HttpStatusCode.OK, null!, _testMovie);
            _mediator.Setup(x => x.Send(It.IsAny<UpdateMovieCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResponse);

            var request = new UpdateMovieRequest()
            {
                Id = _testMovie.Id,
                AvailableQuantity = _testMovie.AvailableQuantity,
                Name = _testMovie.Name,
                PricePerDay = _testMovie.PricePerDay,
                Year = _testMovie.Year,
                TotalQuantity = _testMovie.TotalQuantity,
            };

            //Act

            var result = await _controller.Update(request, new CancellationToken());

            //Assert

            AssertOkObjectResult(result, mediatorResponse.Model!);
        }

        [Fact]
        public async Task Movies_Update_NotFoundPath()
        {
            //Arrange

            var mediatorResponse = GenerateResponse<MovieResponse, Movie, MovieDto>(HttpStatusCode.NotFound, "d", default(Movie)!);
            _mediator.Setup(x => x.Send(It.IsAny<UpdateMovieCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResponse);

            var request = new UpdateMovieRequest()
            {
                Id = _testMovie.Id,
                AvailableQuantity = _testMovie.AvailableQuantity,
                Name = _testMovie.Name,
                PricePerDay = _testMovie.PricePerDay,
                Year = _testMovie.Year,
                TotalQuantity = _testMovie.TotalQuantity,
            };

            //Act

            var result = await _controller.Update(request, new CancellationToken());

            //Assert

            AssertNotFoundObjectResult(result, mediatorResponse.Message);
        }

        [Fact]
        public async Task Movies_Delete_SuccessfullPath()
        {
            //Arrange

            var mediatorResponse = GenerateResponse<MovieResponse, Movie, MovieDto>(HttpStatusCode.NoContent, null!, default(Movie)!);
            _mediator.Setup(x => x.Send(It.IsAny<DeleteMovieCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResponse);

            var request = new ByIntIdRequest()
            {
                Id = _testMovie.Id,
            };

            //Act

            var result = await _controller.Delete(request, new CancellationToken());

            //Assert

            AssertNoContentResult(result);
        }

        [Fact]
        public async Task Movies_Delete_NotFoundPath()
        {
            //Arrange

            var mediatorResponse = GenerateResponse<MovieResponse, Movie, MovieDto>(HttpStatusCode.NotFound, "d", default(Movie)!);
            _mediator.Setup(x => x.Send(It.IsAny<DeleteMovieCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResponse);

            var request = new ByIntIdRequest()
            {
                Id = _testMovie.Id,
            };

            //Act

            var result = await _controller.Delete(request, new CancellationToken());

            //Assert

            AssertNotFoundObjectResult(result, mediatorResponse.Message);
        }

        [Fact]
        public async Task Movies_GetAll_SuccessfullPath()
        {
            //Arrange

            var mediatorResponse = GenerateResponse<MovieCollectionResponse, IEnumerable<Movie>, IEnumerable<MovieDto>>(HttpStatusCode.OK, null!, _testMovies);
            _mediator.Setup(x => x.Send(It.IsAny<GetAllMoviesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResponse);

           
            //Act

            var result = await _controller.GetAll(new CancellationToken());

            //Assert

            AssertOkObjectResult(result,mediatorResponse.Model!);
        }

        [Fact]
        public async Task Movies_GetById_SuccessfullPath()
        {
            //Arrange

            var mediatorResponse = GenerateResponse<MovieResponse, Movie, MovieDto>(HttpStatusCode.OK, null!, _testMovie);
            _mediator.Setup(x => x.Send(It.IsAny<GetMovieByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResponse);

            var request = new ByIntIdRequest()
            {
                Id = _testMovie.Id
            };

            //Act

            var result = await _controller.GetById(request,new CancellationToken());

            //Assert

            AssertOkObjectResult(result, mediatorResponse.Model!);
        }

        [Fact]
        public async Task Movies_GetById_NotFoundPath()
        {
            //Arrange

            var mediatorResponse = GenerateResponse<MovieResponse, Movie, MovieDto>(HttpStatusCode.NotFound, "d", default(Movie)!);
            _mediator.Setup(x => x.Send(It.IsAny<GetMovieByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResponse);

            var request = new ByIntIdRequest()
            {
                Id = _testMovie.Id,
            };

            //Act

            var result = await _controller.GetById(request, new CancellationToken());

            //Assert

            AssertNotFoundObjectResult(result, mediatorResponse.Message);
        }

        [Fact]
        public async Task Movies_AddInventory_SuccessfullPath()
        {
            //Arrange

            var mediatorResponse = GenerateResponse<MovieResponse, Movie, MovieDto>(HttpStatusCode.OK, null!, _testMovie);
            _mediator.Setup(x => x.Send(It.IsAny<AddInventoryCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResponse);

            var request = new AddInventoryRequest()
            {
                MovieId = _testMovie.Id,
                Quantity = 1,
            };

            //Act

            var result = await _controller.AddInventory(request, new CancellationToken());

            //Assert

            AssertOkObjectResult(result, mediatorResponse.Model!);
        }

        [Fact]
        public async Task Movies_AddInventory_NotFoundPath()
        {
            //Arrange

            var mediatorResponse = GenerateResponse<MovieResponse, Movie, MovieDto>(HttpStatusCode.NotFound, "d", default(Movie)!);
            _mediator.Setup(x => x.Send(It.IsAny<AddInventoryCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResponse);

            var request = new AddInventoryRequest()
            {
                MovieId = _testMovie.Id,
                Quantity = 1,
            };

            //Act

            var result = await _controller.AddInventory(request, new CancellationToken());

            //Assert

            AssertNotFoundObjectResult(result, mediatorResponse.Message);
        }

        [Fact]
        public async Task Movies_RemoveInventory_SuccessfullPath()
        {
            //Arrange

            var mediatorResponse = GenerateResponse<MovieResponse, Movie, MovieDto>(HttpStatusCode.OK, null!, _testMovie);
            _mediator.Setup(x => x.Send(It.IsAny<RemoveInventoryCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResponse);

            var request = new RemoveInventoryRequest()
            {
                MovieId = _testMovie.Id,
                Quantity = 1,
            };

            //Act

            var result = await _controller.RemoveInventory(request, new CancellationToken());

            //Assert

            AssertOkObjectResult(result, mediatorResponse.Model!);
        }

        [Fact]
        public async Task Movies_RemoveInventory_NotFoundPath()
        {
            //Arrange

            var mediatorResponse = GenerateResponse<MovieResponse, Movie, MovieDto>(HttpStatusCode.NotFound, "d", default(Movie)!);
            _mediator.Setup(x => x.Send(It.IsAny<RemoveInventoryCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResponse);

            var request = new RemoveInventoryRequest()
            {
                MovieId = _testMovie.Id,
                Quantity = 1,
            };

            //Act

            var result = await _controller.RemoveInventory(request, new CancellationToken());

            //Assert

            AssertNotFoundObjectResult(result, mediatorResponse.Message);
        }


        [Fact]
        public async Task Movies_RemoveInventory_BadRequestPath()
        {
            //Arrange

            var mediatorResponse = GenerateResponse<MovieResponse, Movie, MovieDto>(HttpStatusCode.BadRequest, "d", default(Movie)!);
            _mediator.Setup(x => x.Send(It.IsAny<RemoveInventoryCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResponse);

            var request = new RemoveInventoryRequest()
            {
                MovieId = _testMovie.Id,
                Quantity = 1,
            };

            //Act

            var result = await _controller.RemoveInventory(request, new CancellationToken());

            //Assert

            AssertBadRequestObjectResult(result, mediatorResponse.Message);
        }
    }
}
