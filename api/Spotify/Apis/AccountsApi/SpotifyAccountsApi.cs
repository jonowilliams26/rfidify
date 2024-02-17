using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using RFIDify.Services;
using RFIDify.Spotify.Apis.AccountsApi.RequestResponse;

namespace RFIDify.Spotify.Apis.AccountsApi;

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
    public Uri GenerateAuthorizationUri(SpotifyCredentials credentials, SpotifyAuthorizationState authorizationState)
    {
        var uri = httpClient.BaseAddress!.AbsoluteUri.Replace("/api", "/authorize");

        var query = new Dictionary<string, string?>
        {
            [FormParams.ClientId] = credentials.ClientId,
            [FormParams.ResponseType] = ResponseTypes.Code,
            [FormParams.RedirectUri] = authorizationState.RedirectUri,
            [FormParams.State] = authorizationState.State,
            [FormParams.Scope] = options.CurrentValue.Scopes,
            [FormParams.ShowDialog] = "true"
        };

        return new Uri(QueryHelpers.AddQueryString(uri, query));
    }

    public async Task<SpotifyTokens> ExchangeAuthorizationCodeForTokens(string authorizationCode, SpotifyAuthorizationState authorizationState, CancellationToken cancellationToken)
    {
        var request = new ExchangeAuthorizationCodeForTokensRequest(authorizationCode, authorizationState.RedirectUri);
        var response = await httpClient.PostAsFormUrlEncoded<ExchangeAuthorizationCodeForTokensResponse>(request, cancellationToken);

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
        var request = new RefreshAccessTokenRequest(refreshToken.Token);
        var response = await httpClient.PostAsFormUrlEncoded<RefreshAccessTokenResponse>(request, cancellationToken);
        return new SpotifyAccessToken
        {
            Token = response.AccessToken,
            ExpiresAtUtc = dateTimeProvider.UtcNow.AddSeconds(response.ExpiresIn)
        };
    }
}