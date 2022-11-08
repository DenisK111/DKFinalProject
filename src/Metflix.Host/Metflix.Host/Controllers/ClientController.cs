using System.Net;
using MediatR;
using Metflix.Host.Common;
using Metflix.Host.Extensions;
using Metflix.Models.Mediatr.Commands.Purchases;
using Metflix.Models.Mediatr.Queries.Movies;
using Metflix.Models.Mediatr.Queries.Purchases;
using Metflix.Models.Requests.Purchase;
using Metflix.Models.Responses.Purchases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Metflix.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetAvailableMovies))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableMovies(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAvailableMoviesQuery(),cancellationToken);
            return this.ProduceResponse(result);
        }

        [HttpGet(nameof(GetMyPurchases))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyPurchases(CancellationToken cancellationToken)
        {
            var userId = this.GetUserId();
            var result = await _mediator.Send(new GetMyPurchasesQuery(userId),cancellationToken);
            return this.ProduceResponse(result);
        }

        [HttpPost(nameof(MakePurchase))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MakePurchase([FromBody] PurchaseRequest request, CancellationToken cancellationToken)
        {            
            var userId = this.GetUserId();
            var result = await _mediator.Send(new MakePurchaseCommand(request, userId),cancellationToken);
                        
            if (result.HttpStatusCode==HttpStatusCode.Created)
            {
                return CreatedAtAction(nameof(GetPurchaseById), new { Id = result.Model!.Id }, result.Model);
            }

            return this.ProduceResponse(result);
        }
        [HttpPost(nameof(ReturnMovie))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReturnMovie([FromQuery]int userMovieId, CancellationToken cancellationToken)
        {
            var userId = this.GetUserId();
            var result = await _mediator.Send(new ReturnMovieCommand(userMovieId, userId), cancellationToken);                     
            return this.ProduceResponse(result);
        }

        [HttpGet(nameof(GetPurchaseById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPurchaseById(Guid id, CancellationToken cancellationToken)
        {
            var userId = this.GetUserId();
            var result = await _mediator.Send(new GetPurchaseByIdAndUserIdQuery(id,userId), cancellationToken);
            return this.ProduceResponse(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(nameof(GetCurrentlyTakenMovies))]
        public async Task<IActionResult> GetCurrentlyTakenMovies(CancellationToken cancellationToken)
        {
            var userId = this.GetUserId();
            var result = await _mediator.Send(new GetCurrentlyTakenMoviesQuery(userId), cancellationToken);
            return this.ProduceResponse(result);
        }
    }   
}
