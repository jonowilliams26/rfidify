using Microsoft.AspNetCore.Diagnostics;

namespace RFIDify.Api.ExceptionHandlers;

public class UnhandledExceptionHandler(ILogger<UnhandledExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred.");
        httpContext.Response.StatusCode = 500;
        await httpContext.Response.WriteAsJsonAsync(new
        {
            Message = "Sorry, something went wrong",
        }, cancellationToken);
        return true;
    }
}
