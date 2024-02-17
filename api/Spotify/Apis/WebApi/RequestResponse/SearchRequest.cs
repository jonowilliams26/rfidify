namespace RFIDify.Spotify.Apis.WebApi.RequestResponse;

public record SearchRequest(string Search, SpotifyItemType Type) : ISpotifyRequest
{
    public string Uri()
    {
        var query = new Dictionary<string, string?>
        {
            [QueryParams.Search] = Search,
            [QueryParams.Type] = Type.ToString()
        };

        return $"search{QueryString.Create(query)}";
    }
}

public record SearchResponse
{
    public SpotifyPagedResponse<SpotifyTrack>? Tracks { get; init; }
    public SpotifyPagedResponse<SpotifyAlbum>? Albums { get; init; }
    public SpotifyPagedResponse<SpotifyArtist>? Artists { get; init; }
    public SpotifyPagedResponse<SpotifyPlaylist>? Playlists { get; init; }
}