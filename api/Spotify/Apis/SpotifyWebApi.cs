using HtmlAgilityPack;

namespace RFIDify.Spotify.Apis;

public interface ISpotifyWebApi
{
    Task<SpotifyItem> Get(SpotifyUri uri, CancellationToken cancellationToken);
    Task<SpotifyAlbum> GetAlbum(string id, CancellationToken cancellationToken);
    Task<SpotifyArtist> GetArtist(string id, CancellationToken cancellationToken);
    Task<SpotifyPlaylist> GetPlaylist(string id, CancellationToken cancellationToken);
    Task<SpotifyPagedResponse<SpotifyPlaylist>> GetPlaylists(int? offset, CancellationToken cancellationToken);
    Task<SpotifyPagedResponse<SpotifyArtist>> GetTopArtists(int? offset, CancellationToken cancellationToken);
    Task<SpotifyPagedResponse<SpotifyTrack>> GetTopTracks(int? offset, CancellationToken cancellationToken);
    Task<SpotifyPagedResponse<SpotifyAlbum>> GetSavedAlbums(int? offset, CancellationToken cancellationToken);
    Task<SpotifyTrack> GetTrack(string id, CancellationToken cancellationToken);
    Task<SpotifyPagedResponse<SpotifyItem>> Search(string search, SpotifyItemType type, CancellationToken cancellationToken);
    Task Play(SpotifyItem item, CancellationToken cancellationToken);
}

public class SpotifyWebApi(HttpClient httpClient) : ISpotifyWebApi
{
    private readonly SpotifyHttpClient httpClient = new(httpClient);

    public async Task Play(SpotifyItem item, CancellationToken cancellationToken)
    {
        PlayRequest request = item switch
        {
            SpotifyTrack => new() { Uris = [item.Uri] },
            _ => new() { ContextUri = item.Uri }
        };

        await httpClient.Put("me/player/play", request, cancellationToken);
    }

    public async Task<SpotifyItem> Get(SpotifyUri uri, CancellationToken cancellationToken) => uri.Type switch
    {
        SpotifyItemType.track => await GetTrack(uri.Id, cancellationToken),
        SpotifyItemType.album => await GetAlbum(uri.Id, cancellationToken),
        SpotifyItemType.artist => await GetArtist(uri.Id, cancellationToken),
        SpotifyItemType.playlist => await GetPlaylist(uri.Id, cancellationToken),
        _ => throw new NotImplementedException()
    };

    public async Task<SpotifyTrack> GetTrack(string id, CancellationToken cancellationToken)
    {
        return await httpClient.Get<SpotifyTrack>($"tracks/{id}", cancellationToken);
    }

    public async Task<SpotifyPlaylist> GetPlaylist(string id, CancellationToken cancellationToken)
    {
        var query = new Dictionary<string, string?>
        {
            ["fields"] = "id,uri,name,description,images",
        };

        var uri = $"playlists/{id}{QueryString.Create(query)}";
        return await httpClient.Get<SpotifyPlaylist>(uri, cancellationToken);
    }

    public async Task<SpotifyAlbum> GetAlbum(string id, CancellationToken cancellationToken)
    {
        return await httpClient.Get<SpotifyAlbum>($"albums/{id}", cancellationToken);
    }

    public async Task<SpotifyArtist> GetArtist(string id, CancellationToken cancellationToken)
    {
        return await httpClient.Get<SpotifyArtist>($"artists/{id}", cancellationToken);
    }

    public async Task<SpotifyPagedResponse<SpotifyTrack>> GetTopTracks(int? offset, CancellationToken cancellationToken)
    {
        var uri = CreateTopItemRequestUri("me/top/tracks", offset);
        return await httpClient.Get<SpotifyPagedResponse<SpotifyTrack>>(uri, cancellationToken);
    }

    public async Task<SpotifyPagedResponse<SpotifyArtist>> GetTopArtists(int? offset, CancellationToken cancellationToken)
    {
        var uri = CreateTopItemRequestUri("me/top/artists", offset);
        return await httpClient.Get<SpotifyPagedResponse<SpotifyArtist>>(uri, cancellationToken);
    }

    public async Task<SpotifyPagedResponse<SpotifyPlaylist>> GetPlaylists(int? offset, CancellationToken cancellationToken)
    {
        var uri = CreateTopItemRequestUri("me/playlists", offset);
        var response = await httpClient.Get<SpotifyPagedResponse<SpotifyPlaylist>>(uri, cancellationToken);
        return new SpotifyPagedResponse<SpotifyPlaylist>
        {
            Items = response.Items.Select(RemoveHTMLFromPlaylist).ToList(),
            Total = response.Total,
            Limit = response.Limit,
            Next = response.Next,
            Offset = response.Offset,
            Previous = response.Previous
        };
    }

    private static SpotifyPlaylist RemoveHTMLFromPlaylist(SpotifyPlaylist playlist)
    {
        if (playlist.Description is null)
        {
            return playlist;
        }

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(playlist.Description);

        if (htmlDoc == null)
        {
            return playlist;
        }

        playlist.Description = htmlDoc.DocumentNode.InnerText;
        return playlist;
    }

    public async Task<SpotifyPagedResponse<SpotifyAlbum>> GetSavedAlbums(int? offset, CancellationToken cancellationToken)
    {
        var uri = CreateTopItemRequestUri("me/albums", offset);
        var response = await httpClient.Get<SpotifyPagedResponse<GetSavedAlbumsResponseItem>>(uri, cancellationToken);
        return new SpotifyPagedResponse<SpotifyAlbum>
        {
            Items = response.Items.Select(x => x.Album).ToList(),
            Total = response.Total,
            Limit = response.Limit,
            Next = response.Next,
            Offset = response.Offset,
            Previous = response.Previous
        };
    }

    private static string CreateTopItemRequestUri(string uri, int? offset)
    {
        var query = new Dictionary<string, string?>
        {
            ["offset"] = offset?.ToString() ?? "0",
            ["limit"] = "20",
            ["time_range"] = "short_term"
        };

        return $"{uri}{QueryString.Create(query)}";
    }

    public async Task<SpotifyPagedResponse<SpotifyItem>> Search(string search, SpotifyItemType type, CancellationToken cancellationToken)
    {
        var query = new Dictionary<string, string?>
        {
            ["q"] = search,
            ["type"] = type.ToString(),
            ["limit"] = "20"
        };

        var uri = $"search{QueryString.Create(query)}";
        var response = await httpClient.Get<SearchResponse>(uri, cancellationToken);

        return type switch
        {
            SpotifyItemType.track => CreatePagedResponseFromSearchResponse(response.Tracks!),
            SpotifyItemType.album => CreatePagedResponseFromSearchResponse(response.Albums!),
            SpotifyItemType.artist => CreatePagedResponseFromSearchResponse(response.Artists!),
            SpotifyItemType.playlist => CreatePagedResponseFromSearchResponse(response.Playlists!),
            _ => throw new NotImplementedException()
        };
    }

    private static SpotifyPagedResponse<SpotifyItem> CreatePagedResponseFromSearchResponse<T>(SpotifyPagedResponse<T> response) where T : SpotifyItem
    {
        var items = response.Items switch
        {
            List<SpotifyPlaylist> playlists => playlists.Select(RemoveHTMLFromPlaylist).ToList<SpotifyItem>(),
            _ => [.. response.Items]
        };

        return new()
        {
            Items = items,
            Total = response.Total,
            Limit = response.Limit,
            Next = response.Next,
            Offset = response.Offset,
            Previous = response.Previous
        };
    }

    private record PlayRequest
    {
        public string? ContextUri { get; init; }
        public string[]? Uris { get; init; }
    }

    private record GetSavedAlbumsResponseItem
    {
        public required SpotifyAlbum Album { get; init; }
    }

    private record SearchResponse
    {
        public SpotifyPagedResponse<SpotifyTrack>? Tracks { get; init; }
        public SpotifyPagedResponse<SpotifyAlbum>? Albums { get; init; }
        public SpotifyPagedResponse<SpotifyArtist>? Artists { get; init; }
        public SpotifyPagedResponse<SpotifyPlaylist>? Playlists { get; init; }
    }
}