﻿namespace RFIDify.Spotify.Apis;

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
    Task Play(SpotifyItem item, CancellationToken cancellationToken);
}

public class SpotifyWebApi(HttpClient httpClient) : ISpotifyWebApi
{
    private readonly SpotifyHttpClient httpClient = new(httpClient);

    public async Task Play(SpotifyItem item, CancellationToken cancellationToken)
    {
        PlayRequest request = item.Type switch
        {
            SpotifyItemType.Track => new() { Uris = [item.Uri] },
            _ => new() { ContextUri = item.Uri }
        };

        await httpClient.Put("me/player/play", request, cancellationToken);
    }

    public async Task<SpotifyItem> Get(SpotifyUri uri, CancellationToken cancellationToken) => uri.Type switch
    {
        SpotifyItemType.Track => await GetTrack(uri.Id, cancellationToken),
        SpotifyItemType.Album => await GetAlbum(uri.Id, cancellationToken),
        SpotifyItemType.Artist => await GetArtist(uri.Id, cancellationToken),
        SpotifyItemType.Playlist => await GetPlaylist(uri.Id, cancellationToken),
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
        return await httpClient.Get<SpotifyPagedResponse<SpotifyPlaylist>>(uri, cancellationToken);
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

    private record PlayRequest
    {
        public string? ContextUri { get; init; }
        public string[]? Uris { get; init; }
    }

    private record GetSavedAlbumsResponseItem
    {
        public required SpotifyAlbum Album { get; init; }
    }
}