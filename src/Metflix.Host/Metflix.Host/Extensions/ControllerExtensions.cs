using Metflix.Host.Common.Jwt;
using Metflix.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Metflix.Host.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ProduceResponse<T>(this ControllerBase controller, BaseResponse<T> response,string actionName = default!,object routeValues = default!)
        {
            switch (response.HttpStatusCode)
            {
                case HttpStatusCode.OK:
                    return controller.Ok(response.Model);
                case HttpStatusCode.NotFound:
                    return controller.NotFound(new ErrorResponse() { Error = response.Message });
                case HttpStatusCode.BadRequest:
                    return controller.BadRequest(new ErrorResponse() { Error = response.Message });
                case HttpStatusCode.Conflict:
                    return controller.Conflict(new ErrorResponse() { Error = response.Message });
                case HttpStatusCode.NoContent:
                    return controller.NoContent();
                case HttpStatusCode.Created:
                    return controller.CreatedAtAction(actionName,routeValues, response.Model);
                default: return controller.StatusCode(500);
            }
        }

        public static string GetUserId(this ControllerBase controller)
        {
            return controller.User?.FindFirst(JwtClaims.Id)?.Value!;
        }
    }
}
