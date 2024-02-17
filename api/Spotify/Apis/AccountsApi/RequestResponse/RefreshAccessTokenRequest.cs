namespace RFIDify.Spotify.Apis.AccountsApi.RequestResponse;

public record RefreshAccessTokenRequest(string RefreshToken) : ISpotifyRequestFormUrlEncodeable
{
    public string Uri() => "token";
    public Dictionary<string, string> FormContent() => new()
    {
        [FormParams.GrantType] = GrantTypes.RefreshToken,
        [FormParams.RefreshToken] = RefreshToken
    };
}

public record RefreshAccessTokenResponse
{
    public required string AccessToken { get; set; }
    public required int ExpiresIn { get; set; }
}
