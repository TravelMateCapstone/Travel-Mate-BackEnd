using BusinessObjects.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace TravelMateAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Log the exception details for debugging
            _logger.LogError(exception, exception.Message);

            ErrorDetails errorDetails = new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            };

            if (exception is DuplicateException duplicationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                errorDetails.Message = duplicationException.Message;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            // Serialize error details to JSON
            var jsonResponse = JsonConvert.SerializeObject(errorDetails);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
