using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace RFIDify.Api.ExceptionHandlers;

public class SpotifyRequestExceptionHandler(ILogger<SpotifyRequestExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is HttpRequestException httpException)
        {
            logger.LogError(httpException, "An error occurred while making a request to the Spotify API.");
            httpContext.Response.StatusCode = (int)(httpException.StatusCode ?? HttpStatusCode.InternalServerError);
            await httpContext.Response.WriteAsJsonAsync(new
            {
                Message = "An error occurred while making a request to the Spotify API.",
            }, cancellationToken);
            return true;
        }

        return false;
    }
}
