using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Net;
using Utils;

namespace Metflix.Host.Middleware.ErrorHandlerMiddleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    AppException e => (int)HttpStatusCode.BadRequest,
                    KeyNotFoundException e => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                var result = new
                {
                    message = error switch
                    {
                        SqlException=> ResponseMessages.SqlExceptionMessage,
                        AppException e => e.Message,
                        KeyNotFoundException e => e.Message,
                        _ => ResponseMessages.InternalServerErrorMessage
                    }
                };

                _logger.LogError(error.StackTrace);
                await response.WriteAsJsonAsync(result);
            }
        }
    }
}
