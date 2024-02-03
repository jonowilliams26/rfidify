using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using RFIDify.Services;

namespace RFIDify.Spotify.Apis;

public interface ISpotifyAccountsApi
{
    Task<SpotifyTokens> ExchangeAuthorizationCodeForTokens(string authorizationCode, SpotifyAuthorizationState authorizationState, CancellationToken cancellationToken);
    Uri GenerateAuthorizationUri(SpotifyCredentials credentials, SpotifyAuthorizationState authorizationState);
    Task<SpotifyAccessToken> RefreshAccessToken(SpotifyRefreshToken refreshToken, CancellationToken cancellationToken);
}

public class SpotifyAccountsApiOptions
{
    public required string Scopes { get; init; }
}

public class SpotifyAccountsApi(HttpClient httpClient, IOptionsMonitor<SpotifyAccountsApiOptions> options, IDateTimeProvider dateTimeProvider) : ISpotifyAccountsApi
{
    private readonly SpotifyHttpClient httpClient = new(httpClient);

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

    public async Task<SpotifyTokens> ExchangeAuthorizationCodeForTokens(string authorizationCode, SpotifyAuthorizationState authorizationState, CancellationToken cancellationToken)
    {
        var request = new Dictionary<string, string>
        {
            [Parameters.RedirectUri] = authorizationState.RedirectUri,
            [Parameters.Code] = authorizationCode,
            [Parameters.GrantType] = GrantTypes.AuthorizationCode
        };

        var response = await httpClient.Post<ExchangeAuthorizationCodeForTokensResponse>("token", request, cancellationToken);

        var accessToken = new SpotifyAccessToken
        {
            Token = response.AccessToken,
            ExpiresAtUtc = dateTimeProvider.UtcNow.AddSeconds(response.ExpiresIn)
        };

        var refreshToken = new SpotifyRefreshToken
        {
            Token = response.RefreshToken
        };

        return new SpotifyTokens(accessToken, refreshToken);
    }

    public async Task<SpotifyAccessToken> RefreshAccessToken(SpotifyRefreshToken refreshToken, CancellationToken cancellationToken)
    {
        var request = new Dictionary<string, string>
        {
            [Parameters.GrantType] = GrantTypes.RefreshToken,
            [Parameters.RefreshToken] = refreshToken.Token
        };

        var response = await httpClient.Post<RefreshAccessTokenResponse>("token", request, cancellationToken);
        return new SpotifyAccessToken
        {
            Token = response.AccessToken,
            ExpiresAtUtc = dateTimeProvider.UtcNow.AddSeconds(response.ExpiresIn)
        };
    }

    private static class Parameters
    {
        public const string ClientId = "client_id";
        public const string ResponseType = "response_type";
        public const string RedirectUri = "redirect_uri";
        public const string State = "state";
        public const string Scope = "scope";
        public const string ShowDialog = "show_dialog";
        public const string Code = "code";
        public const string GrantType = "grant_type";
        public const string RefreshToken = "refresh_token";
    }

    private static class ResponseTypes
    {
        public const string Code = "code";
    }

    private static class GrantTypes
    {
        public const string AuthorizationCode = "authorization_code";
        public const string RefreshToken = "refresh_token";
    }

    private record ExchangeAuthorizationCodeForTokensResponse
    {
        public required string AccessToken { get; set; }
        public required int ExpiresIn { get; set; }
        public required string RefreshToken { get; set; }
    }

    private record RefreshAccessTokenResponse
    {
        public required string AccessToken { get; set; }
        public required int ExpiresIn { get; set; }
    }
}