using System.ComponentModel.DataAnnotations;

namespace BankMin_API.Startup.Middlewares
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
        public async Task Invoke(HttpContext httpContext)
        {
            try { await _next(httpContext); }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = ex switch
                {
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    ValidationException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };
                httpContext.Response.ContentType = "application/json";

                var body = new
                {
                    error = ex.Message
                };

                var json = System.Text.Json.JsonSerializer.Serialize(body);
                await httpContext.Response.WriteAsync(json);
                _logger.LogError($"\n---------Handled exception:----------\n{ex.Message}\n-------------------------------------");
            }
        }
    }
}
