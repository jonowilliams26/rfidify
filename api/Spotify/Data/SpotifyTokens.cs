namespace RFIDify.Spotify.Data;

public record SpotifyTokens(SpotifyAccessToken AccessToken, SpotifyRefreshToken RefreshToken);

public class SpotifyAccessToken
{
    public required string Token { get; init; }
    public DateTime ExpiresAtUtc { get; init; }
}

public class SpotifyRefreshToken
{
    public required string Token { get; init; }
}
