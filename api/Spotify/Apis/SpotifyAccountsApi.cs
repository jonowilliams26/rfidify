using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using RFIDify.Spotify.Data;

namespace RFIDify.Spotify.Apis;

public interface ISpotifyAccountsApi
{
    Uri GenerateAuthorizationUri(SpotifyCredentials credentials, SpotifyAuthorizationState authorizationState);
}

public class SpotifyAccountsApiOptions
{
    public required string Scopes { get; init; }
}

public class SpotifyAccountsApi(HttpClient httpClient, IOptionsMonitor<SpotifyAccountsApiOptions> options) : ISpotifyAccountsApi
{
    public Uri GenerateAuthorizationUri(SpotifyCredentials credentials, SpotifyAuthorizationState authorizationState)
    {
        var uri = httpClient.BaseAddress!.AbsoluteUri.Replace("/api", "/authorize");

        var query = new Dictionary<string, string?>
        {
            [Parameters.ClientId] = credentials.ClientId,
            [Parameters.ResponseType] = ResponseTypes.Code,
            [Parameters.RedirectUri] = authorizationState.RedirectUri,
            [Parameters.State] = authorizationState.State,
            [Parameters.Scope] = options.CurrentValue.Scopes,
            [Parameters.ShowDialog] = "true"
        };

        return new Uri(QueryHelpers.AddQueryString(uri, query));
    }

    private static class Parameters
    {
        public const string ClientId = "client_id";
        public const string ResponseType = "response_type";
        public const string RedirectUri = "redirect_uri";
        public const string State = "state";
        public const string Scope = "scope";
        public const string ShowDialog = "show_dialog";
    }

    private static class ResponseTypes
    {
        public const string Code = "code";
    }
}
