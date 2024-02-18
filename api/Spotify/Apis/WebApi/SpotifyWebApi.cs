using HtmlAgilityPack;
using RFIDify.Spotify.Apis.WebApi.RequestResponse;
using RFIDify.Spotify.Apis.WebApi.RequestResponse.Items;

namespace RFIDify.Spotify.Apis.WebApi;

public interface ISpotifyWebApi
{
    Task<SpotifyItem> Get(SpotifyUri uri, CancellationToken cancellationToken);
    Task<Album> GetAlbum(string id, CancellationToken cancellationToken);
    Task<Artist> GetArtist(string id, CancellationToken cancellationToken);
    Task<Playlist> GetPlaylist(string id, CancellationToken cancellationToken);
    Task<PagedResponse<Playlist>> GetPlaylists(int? offset, CancellationToken cancellationToken);
    Task<PagedResponse<Artist>> GetTopArtists(int? offset, CancellationToken cancellationToken);
    Task<PagedResponse<Track>> GetTopTracks(int? offset, CancellationToken cancellationToken);
    Task<PagedResponse<Album>> GetAlbums(int? offset, CancellationToken cancellationToken);
    Task<Track> GetTrack(string id, CancellationToken cancellationToken);
    Task<PagedResponse<SpotifyItem>> Search(string search, SpotifyItemType type, int? offset, CancellationToken cancellationToken);
    Task Play(SpotifyItem item, CancellationToken cancellationToken);
}

public class SpotifyWebApi(HttpClient httpClient) : ISpotifyWebApi
{
    public async Task Play(SpotifyItem item, CancellationToken cancellationToken)
    {
        var request = new PlayRequest(item);
        await httpClient.PutAsJsonAsync(request, cancellationToken);
    }

    public async Task<SpotifyItem> Get(SpotifyUri uri, CancellationToken cancellationToken) => uri.Type switch
    {
        SpotifyItemType.track => await GetTrack(uri.Id, cancellationToken),
        SpotifyItemType.album => await GetAlbum(uri.Id, cancellationToken),
        SpotifyItemType.artist => await GetArtist(uri.Id, cancellationToken),
        SpotifyItemType.playlist => await GetPlaylist(uri.Id, cancellationToken),
        _ => throw new NotImplementedException()
    };

    public async Task<Track> GetTrack(string id, CancellationToken cancellationToken)
    {
        var request = new GetTrackRequest(id);
        return await httpClient.GetFromJsonAsync<Track>(request, cancellationToken);
    }

    public async Task<Playlist> GetPlaylist(string id, CancellationToken cancellationToken)
    {
        var request = new GetPlaylistRequest(id);
        return await httpClient.GetFromJsonAsync<Playlist>(request, cancellationToken);
    }

    public async Task<Album> GetAlbum(string id, CancellationToken cancellationToken)
    {
        var request = new GetAlbumRequest(id);
        return await httpClient.GetFromJsonAsync<Album>(request, cancellationToken);
    }

    public async Task<Artist> GetArtist(string id, CancellationToken cancellationToken)
    {
        var request = new GetArtistRequest(id);
        return await httpClient.GetFromJsonAsync<Artist>(request, cancellationToken);
    }

    public async Task<PagedResponse<Track>> GetTopTracks(int? offset, CancellationToken cancellationToken)
    {
        var request = new GetTopTracksRequest(offset);
        return await httpClient.GetFromJsonAsync<PagedResponse<Track>>(request, cancellationToken);
    }

    public async Task<PagedResponse<Artist>> GetTopArtists(int? offset, CancellationToken cancellationToken)
    {
        var request = new GetTopArtistsRequest(offset);
        return await httpClient.GetFromJsonAsync<PagedResponse<Artist>>(request, cancellationToken);
    }

    public async Task<PagedResponse<Playlist>> GetPlaylists(int? offset, CancellationToken cancellationToken)
    {
        var request = new GetPlaylistsRequests(offset);
        var response = await httpClient.GetFromJsonAsync<PagedResponse<Playlist>>(request, cancellationToken);
        return new PagedResponse<Playlist>
        {
            Items = response.Items.Select(RemoveHTMLFromPlaylist).ToList(),
            Total = response.Total,
            Limit = response.Limit,
            Next = response.Next,
            Offset = response.Offset,
            Previous = response.Previous
        };
    }

    private static Playlist RemoveHTMLFromPlaylist(Playlist playlist)
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

    public async Task<PagedResponse<Album>> GetAlbums(int? offset, CancellationToken cancellationToken)
    {
        var request = new GetAlbumsRequest(offset);
        var response = await httpClient.GetFromJsonAsync<PagedResponse<GetSavedAlbumsResponseItem>>(request, cancellationToken);
        return new PagedResponse<Album>
        {
            Items = response.Items.Select(x => x.Album).ToList(),
            Total = response.Total,
            Limit = response.Limit,
            Next = response.Next,
            Offset = response.Offset,
            Previous = response.Previous
        };
    }

    public async Task<PagedResponse<SpotifyItem>> Search(string search, SpotifyItemType type, int? offset, CancellationToken cancellationToken)
    {
        var request = new SearchRequest(search, type, offset);
        var response = await httpClient.GetFromJsonAsync<SearchResponse>(request, cancellationToken);
        return type switch
        {
            SpotifyItemType.track => CreatePagedResponseFromSearchResponse(response.Tracks!),
            SpotifyItemType.album => CreatePagedResponseFromSearchResponse(response.Albums!),
            SpotifyItemType.artist => CreatePagedResponseFromSearchResponse(response.Artists!),
            SpotifyItemType.playlist => CreatePagedResponseFromSearchResponse(response.Playlists!),
            _ => throw new NotImplementedException()
        };
    }

    private static PagedResponse<SpotifyItem> CreatePagedResponseFromSearchResponse<T>(PagedResponse<T> response) where T : SpotifyItem
    {
        var items = response.Items switch
        {
            List<Playlist> playlists => playlists.Select(RemoveHTMLFromPlaylist).ToList<SpotifyItem>(),
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

    private record GetSavedAlbumsResponseItem
    {
        public required Album Album { get; init; }
    }
}