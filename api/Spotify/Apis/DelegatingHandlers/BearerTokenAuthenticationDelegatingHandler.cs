using RFIDify.Services;
using RFIDify.Spotify.Apis.AccountsApi;
using System.Net;
using System.Net.Http.Headers;

namespace RFIDify.Spotify.Apis.DelegatingHandlers;

public class BearerTokenAuthenticationDelegatingHandler(AppDbContext database, ISpotifyAccountsApi api, ILogger<BearerTokenAuthenticationDelegatingHandler> logger, IDateTimeProvider dateTimeProvider) : DelegatingHandler
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var released = false;
        try
        {
            await semaphore.WaitAsync(cancellationToken);
            var accessToken = await database.SpotifyAccessToken.SingleOrDefaultAsync(cancellationToken);
            if (accessToken is null)
            {
                logger.LogError("No access token found");
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            if (accessToken.ExpiresAtUtc <= dateTimeProvider.UtcNow)
            {
                logger.LogInformation("Access token expired, refreshing...");
                var refreshToken = await database.SpotifyRefreshToken.SingleOrDefaultAsync(cancellationToken);
                if (refreshToken is null)
                {
                    logger.LogError("No refresh token found");
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                var newAccessToken = await api.RefreshAccessToken(refreshToken, cancellationToken);
                await database.SpotifyAccessToken.AddAsync(newAccessToken, cancellationToken);
                database.SpotifyAccessToken.Remove(accessToken);
                await database.SaveChangesAsync(cancellationToken);
                accessToken = newAccessToken;
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Token);
            semaphore.Release();
            released = true;
            return await base.SendAsync(request, cancellationToken);
        }
        finally
        {
            if (!released)
            {
                semaphore.Release();
            }
        }
    }
}