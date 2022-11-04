using MediatR;
using Metflix.Host.Extensions;
using Metflix.Models.Mediatr.Queries.Movies;
using Microsoft.AspNetCore.Mvc;

namespace Metflix.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetAvailableMovies))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAvailableMovies(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAvailableMoviesQuery());
            return this.ProduceResponse(result);
        }
    }
}
