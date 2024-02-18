using RFIDify.Spotify.Apis.WebApi.RequestResponse.Items;

namespace RFIDify.Spotify.Apis.WebApi.RequestResponse;

public record SearchRequest(string Search, SpotifyItemType Type, int? Offset) : ISpotifyRequest
{
    public string Uri()
    {
        var query = new Dictionary<string, string?>
        {
            [QueryParams.Search] = Search,
            [QueryParams.Type] = Type.ToString(),
            [QueryParams.Offset] = Offset?.ToString() ?? "0"
        };

        return $"search{QueryString.Create(query)}";
    }
}

public record SearchResponse
{
    public PagedResponse<Track>? Tracks { get; init; }
    public PagedResponse<Album>? Albums { get; init; }
    public PagedResponse<Artist>? Artists { get; init; }
    public PagedResponse<Playlist>? Playlists { get; init; }
}