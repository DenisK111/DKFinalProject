using Metflix.Models.Common;
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
                    
                    KeyNotFoundException e => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError,
                };


                
                var result = new //TO BE USED FOR END USERS
                {
                    message = error switch
                    {
                        SqlException=> ResponseMessages.SqlExceptionMessage,
                        
                        KeyNotFoundException e => e.Message,
                        _ => ResponseMessages.InternalServerErrorMessage
                    }
                };

                if (error.Data[ExceptionDataKeys.IsCritical] != null)
                {
                    _logger.LogCritical($"{error.Source}\r\n{error.Message}\r\n{error.StackTrace}");
                }
                else
                {
                    _logger.LogError($"{error.Source}\r\n{error.Message}\r\n{error.StackTrace}");
                }
                
                await response.WriteAsJsonAsync($"{error.Source}\r\n{error.Message}\r\n{error.StackTrace}");
            }
        }
    }
}
