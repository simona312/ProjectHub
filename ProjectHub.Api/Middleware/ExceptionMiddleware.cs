using System.Net;
using System.Text.Json;

namespace ProjectHub.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = context.TraceIdentifier;
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var responce = new ErrorResponce
                {
                    Message = "Something went wrong.",
                    StatusCode = context.Response.StatusCode,
                    TraceId = traceId
                };

                if(_env.IsDevelopment())
                {
                    responce.Detail = ex.Message;
                }
                var json = JsonSerializer.Serialize(responce, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                await context.Response.WriteAsync(json);
            }
        }
    }
}
