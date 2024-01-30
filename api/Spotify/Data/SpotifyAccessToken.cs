namespace RFIDify.Spotify.Data;

public class SpotifyAccessToken
{
    public required string Token { get; init; }
    public DateTime ExpiresAtUtc { get; init; }
}
