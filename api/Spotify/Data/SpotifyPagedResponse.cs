namespace RFIDify.Spotify.Data;

public record SpotifyPagedResponse<T>
{
    public Uri? Next { get; init; }
    public Uri? Previous { get; init; }
    public int Limit { get; init; }
    public int Offset { get; init; }
    public int Total { get; init; }
    public List<T> Items { get; init; } = [];
}