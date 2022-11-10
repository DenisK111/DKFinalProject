using System.Net;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Host.Common;
using Metflix.Host.Extensions;
using Metflix.Models.DbModels;
using Metflix.Models.DbModels.Helpers;
using Metflix.Models.Mediatr.Commands;
using Metflix.Models.Mediatr.Commands.Movies;
using Metflix.Models.Mediatr.Queries;
using Metflix.Models.Mediatr.Queries.Movies;
using Metflix.Models.Requests.Movies;
using Metflix.Models.Responses.Movies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Metflix.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = UserRoles.Admin)]
    public class MoviesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MoviesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet(nameof(GetAll))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllMoviesQuery(), token);
            return this.ProduceResponse(result);
        }

        [HttpGet(nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromQuery] ByIntIdRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new GetMovieByIdQuery(request), token);
            return this.ProduceResponse(result);
        }
        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddMovieRequest movie, CancellationToken token)
        {
            var result = await _mediator.Send(new AddMovieCommand(movie), token);
            return this.ProduceResponse(result, nameof(GetById), new { Id = result.Model?.Id });
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateMovieRequest movie, CancellationToken token)
        {
            var result = await _mediator.Send(new UpdateMovieCommand(movie), token);
            return this.ProduceResponse(result);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]ByIntIdRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteMovieCommand(request), token);
            return this.ProduceResponse(result);
        }
        [HttpPut(nameof(AddInventory))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddInventory(AddInventoryRequest request, CancellationToken token)
        {
            var userId = this.GetUserId();
            var result = await _mediator.Send(new AddInventoryCommand(request, userId), token);
            return this.ProduceResponse(result);
        }

        [HttpPut(nameof(RemoveInventory))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveInventory(RemoveInventoryRequest request, CancellationToken token)
        {
            var userId = this.GetUserId();
            var result = await _mediator.Send(new RemoveInventoryCommand(request, userId), token);
            return this.ProduceResponse(result);
        }
    }
}
