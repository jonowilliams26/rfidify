namespace RFIDify.Spotify.Apis.AccountsApi.RequestResponse;

public record ExchangeAuthorizationCodeForTokensRequest(string AuthorizationCode, string RedirectUri) : ISpotifyRequestFormUrlEncodeable
{
    public string Uri() => "token";
    public Dictionary<string, string> FormContent() => new()
    {
        [FormParams.RedirectUri] = RedirectUri,
        [FormParams.Code] = AuthorizationCode,
        [FormParams.GrantType] = GrantTypes.AuthorizationCode
    };
}

public record ExchangeAuthorizationCodeForTokensResponse
{
    public required string AccessToken { get; set; }
    public required int ExpiresIn { get; set; }
    public required string RefreshToken { get; set; }
}