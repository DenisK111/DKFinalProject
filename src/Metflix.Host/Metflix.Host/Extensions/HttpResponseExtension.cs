using Metflix.Host.Controllers;
using Metflix.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Metflix.Host.Extensions
{
    public static class HttpResponseExtensions
    {
        public static IActionResult ProduceResponse<T>(this ControllerBase controller, BaseResponse<T> response)
        {
            switch (response.HttpStatusCode)
            {
                case HttpStatusCode.OK:
                    return controller.Ok(response.Model);
                case HttpStatusCode.NotFound:
                    return controller.NotFound(response.Message);
                case HttpStatusCode.BadRequest:
                    return controller.BadRequest(new ErrorResponse() { Error = response.Message });
                case HttpStatusCode.Conflict:
                    return controller.Conflict(new ErrorResponse() { Error = response.Message });
                case HttpStatusCode.NoContent:
                    return controller.NoContent();
                default: return controller.StatusCode(500);
            }
        }
    }
}
