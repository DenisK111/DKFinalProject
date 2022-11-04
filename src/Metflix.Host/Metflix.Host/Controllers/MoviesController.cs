using System.Net;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Host.Extensions;
using Metflix.Models.DbModels;
using Metflix.Models.Mediatr.Commands;
using Metflix.Models.Mediatr.Queries;
using Metflix.Models.Requests.Movies;
using Microsoft.AspNetCore.Mvc;

namespace Metflix.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            var result = await _mediator.Send(new GetAllMoviesQuery(),token);
            return this.ProduceResponse(result);
        }

        [HttpGet(nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id,CancellationToken token)
        {
            var result = await _mediator.Send(new GetMovieByIdQuery(id),token);
            return this.ProduceResponse(result);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        
        public async Task<IActionResult> Add([FromBody] AddMovieRequest movie, CancellationToken token)
        {
            var result = await _mediator.Send(new AddMovieCommand(movie), token);

            if (result.HttpStatusCode==HttpStatusCode.Created)
            {
                return CreatedAtAction(nameof(GetById), new { Id = result.Model!.Id }, result.Model);
            }

            return this.ProduceResponse(result);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]UpdateMovieRequest movie, CancellationToken token)
        {
            var result = await _mediator.Send(new UpdateMovieCommand(movie), token);
            return this.ProduceResponse(result);
        }
        
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteMovieCommand(id), token);
            return this.ProduceResponse(result);           
        }
    }
}
