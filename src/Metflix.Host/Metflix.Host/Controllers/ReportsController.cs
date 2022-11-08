using MediatR;
using Metflix.Host.Common;
using Metflix.Host.Extensions;
using Metflix.Models.DbModels.Helpers;
using Metflix.Models.Mediatr.Commands.Movies;
using Metflix.Models.Mediatr.Queries.Reports;
using Metflix.Models.Requests;
using Metflix.Models.Responses.Movies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Metflix.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = UserRoles.Admin)]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetInventoryLog))]       
        [ProducesResponseType(StatusCodes.Status200OK)]       
        public async Task<IActionResult> GetInventoryLog(CancellationToken token)
        {            
            var result = await _mediator.Send(new GetInventoryLogQuery(), token);
            return this.ProduceResponse(result);
        }

        public async Task<IActionResult> GetTotalSalesForPeriod(TimePeriodRequest request,CancellationToken token)
        {
            var result = await _mediator.Send(new GetTotalSalesQuery(request), token);
            return this.ProduceResponse(result);
        }
    }
}
