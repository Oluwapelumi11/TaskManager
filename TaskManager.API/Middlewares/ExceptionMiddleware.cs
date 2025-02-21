using TaskManager.API.Model;

namespace TaskManager.API.Middlewares
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }catch (Exception ex) 
            {
                _logger.LogError(ex, "unhandled Exception");
                await HandleExceptionAsync(context, ex);
            } 
        }

        public static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = new ApiResponse<string>(false, "An Occured, Please try again later");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(response);
        }

    }
}
