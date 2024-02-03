namespace RFIDify.Spotify.Apis;

public interface ISpotifyWebApi
{
    Task<ISpotifyItem> Get(SpotifyUri uri, CancellationToken cancellationToken);
    Task<SpotifyAlbum> GetAlbum(string id, CancellationToken cancellationToken);
    Task<SpotifyArtist> GetArtist(string id, CancellationToken cancellationToken);
    Task<SpotifyPlaylist> GetPlaylist(string id, CancellationToken cancellationToken);
    Task<SpotifyTrack> GetTrack(string id, CancellationToken cancellationToken);
    Task Play(ISpotifyItem item, CancellationToken cancellationToken);
}

public class SpotifyWebApi(HttpClient httpClient) : ISpotifyWebApi
{
    private readonly SpotifyHttpClient httpClient = new(httpClient);

    public async Task Play(ISpotifyItem item, CancellationToken cancellationToken)
    {
        PlayRequest request = item.Type switch
        {
            SpotifyItemType.Track => new() { Uris = [item.Uri] },
            _ => new() { ContextUri = item.Uri }
        };

        await httpClient.Put("me/player/play", request, cancellationToken);
    }

    public async Task<ISpotifyItem> Get(SpotifyUri uri, CancellationToken cancellationToken) => uri.Type switch
    {
        SpotifyItemType.Track => await GetTrack(uri.Id, cancellationToken),
        SpotifyItemType.Album => await GetAlbum(uri.Id, cancellationToken),
        SpotifyItemType.Artist => await GetArtist(uri.Id, cancellationToken),
        SpotifyItemType.Playlist => await GetPlaylist(uri.Id, cancellationToken),
        _ => throw new NotImplementedException()
    };

    public Task<SpotifyTrack> GetTrack(string id, CancellationToken cancellationToken) => httpClient.Get<SpotifyTrack>($"tracks/{id}", cancellationToken);
    public Task<SpotifyPlaylist> GetPlaylist(string id, CancellationToken cancellationToken) => httpClient.Get<SpotifyPlaylist>($"playlists/{id}", cancellationToken);
    public Task<SpotifyAlbum> GetAlbum(string id, CancellationToken cancellationToken) => httpClient.Get<SpotifyAlbum>($"albums/{id}", cancellationToken);
    public Task<SpotifyArtist> GetArtist(string id, CancellationToken cancellationToken) => httpClient.Get<SpotifyArtist>($"artists/{id}", cancellationToken);

    private record PlayRequest
    {
        public string? ContextUri { get; init; }
        public string[]? Uris { get; init; }
    }
}
