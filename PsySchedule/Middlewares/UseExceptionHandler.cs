using Microsoft.AspNetCore.Http.HttpResults;

namespace PsySchedule.Middlewares
{
    public class UseExceptionHandler : IMiddleware
    {
        private readonly ILogger<UseExceptionHandler> _logger;

        public UseExceptionHandler(ILogger<UseExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch(OperationCanceledException opEx) 
            {
                _logger.LogError(opEx, $"The request was cancelled. {context.Request.Path}");

                var details = new { message = "Запрос был отменен" };

                context.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;

                await context.Response.WriteAsJsonAsync(details);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{context.Request.Path} exception occurred: {ex.Message}");

                var details = new { message = "Не удалось выполнить запрос" };

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsJsonAsync(details);
            }

        }
    }
}
