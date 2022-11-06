using Microsoft.AspNetCore.Mvc;

namespace Metflix.Host.Common
{
    public static class GetUser
    {
        public static string GetUserId(this ControllerBase controller)
        {
            return controller.User.FindFirst(JwtClaims.Id)?.Value!;
        }
    }
}
