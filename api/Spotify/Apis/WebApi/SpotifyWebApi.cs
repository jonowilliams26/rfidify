using HtmlAgilityPack;
using RFIDify.Spotify.Apis.WebApi.RequestResponse;

namespace RFIDify.Spotify.Apis.WebApi;

public interface ISpotifyWebApi
{
    Task<SpotifyItem> Get(SpotifyUri uri, CancellationToken cancellationToken);
    Task<SpotifyAlbum> GetAlbum(string id, CancellationToken cancellationToken);
    Task<SpotifyArtist> GetArtist(string id, CancellationToken cancellationToken);
    Task<SpotifyPlaylist> GetPlaylist(string id, CancellationToken cancellationToken);
    Task<SpotifyPagedResponse<SpotifyPlaylist>> GetPlaylists(int? offset, CancellationToken cancellationToken);
    Task<SpotifyPagedResponse<SpotifyArtist>> GetTopArtists(int? offset, CancellationToken cancellationToken);
    Task<SpotifyPagedResponse<SpotifyTrack>> GetTopTracks(int? offset, CancellationToken cancellationToken);
    Task<SpotifyPagedResponse<SpotifyAlbum>> GetAlbums(int? offset, CancellationToken cancellationToken);
    Task<SpotifyTrack> GetTrack(string id, CancellationToken cancellationToken);
    Task<SpotifyPagedResponse<SpotifyItem>> Search(string search, SpotifyItemType type, CancellationToken cancellationToken);
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

    public async Task<SpotifyTrack> GetTrack(string id, CancellationToken cancellationToken)
    {
        var request = new GetTrackRequest(id);
        return await httpClient.GetFromJsonAsync<SpotifyTrack>(request, cancellationToken);
    }

    public async Task<SpotifyPlaylist> GetPlaylist(string id, CancellationToken cancellationToken)
    {
        var request = new GetPlaylistRequest(id);
        return await httpClient.GetFromJsonAsync<SpotifyPlaylist>(request, cancellationToken);
    }

    public async Task<SpotifyAlbum> GetAlbum(string id, CancellationToken cancellationToken)
    {
        var request = new GetAlbumRequest(id);
        return await httpClient.GetFromJsonAsync<SpotifyAlbum>(request, cancellationToken);
    }

    public async Task<SpotifyArtist> GetArtist(string id, CancellationToken cancellationToken)
    {
        var request = new GetArtistRequest(id);
        return await httpClient.GetFromJsonAsync<SpotifyArtist>(request, cancellationToken);
    }

    public async Task<SpotifyPagedResponse<SpotifyTrack>> GetTopTracks(int? offset, CancellationToken cancellationToken)
    {
        var request = new GetTopTracksRequest(offset);
        return await httpClient.GetFromJsonAsync<SpotifyPagedResponse<SpotifyTrack>>(request, cancellationToken);
    }

    public async Task<SpotifyPagedResponse<SpotifyArtist>> GetTopArtists(int? offset, CancellationToken cancellationToken)
    {
        var request = new GetTopArtistsRequest(offset);
        return await httpClient.GetFromJsonAsync<SpotifyPagedResponse<SpotifyArtist>>(request, cancellationToken);
    }

    public async Task<SpotifyPagedResponse<SpotifyPlaylist>> GetPlaylists(int? offset, CancellationToken cancellationToken)
    {
        var request = new GetPlaylistsRequests(offset);
        var response = await httpClient.GetFromJsonAsync<SpotifyPagedResponse<SpotifyPlaylist>>(request, cancellationToken);
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

    public async Task<SpotifyPagedResponse<SpotifyAlbum>> GetAlbums(int? offset, CancellationToken cancellationToken)
    {
        var request = new GetAlbumsRequest(offset);
        var response = await httpClient.GetFromJsonAsync<SpotifyPagedResponse<GetSavedAlbumsResponseItem>>(request, cancellationToken);
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

    public async Task<SpotifyPagedResponse<SpotifyItem>> Search(string search, SpotifyItemType type, CancellationToken cancellationToken)
    {
        var request = new SearchRequest(search, type);
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

    private record GetSavedAlbumsResponseItem
    {
        public required SpotifyAlbum Album { get; init; }
    }
}