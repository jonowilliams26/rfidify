using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace RFIDify.Spotify.Apis.DelegatingHandlers;

public class ClientIdAndSecretAuthenticationDelegatingHandler(AppDbContext database, ILogger<ClientIdAndSecretAuthenticationDelegatingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var credentials = await database.SpotifyCredentials
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (credentials is null)
        {
            logger.LogError("No Spotify credentials found in database");
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        var clientIdAndSecret = $"{credentials.ClientId}:{credentials.ClientSecret}";
        var bytes = Encoding.UTF8.GetBytes(clientIdAndSecret);
        var base64Encoded = Convert.ToBase64String(bytes);
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64Encoded);

        return await base.SendAsync(request, cancellationToken);
    }
}
