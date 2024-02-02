namespace RFIDify.Spotify.Endpoints;

public static class SpotifyTestEndpoints
{
    public static void MapSpotifyTestEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/tracks/{id}", async (ISpotifyWebApi api, string id, CancellationToken cancellationToken) =>
        {
            return await api.GetTrack(id, cancellationToken);
        });

        app.MapGet("/albums/{id}", async (ISpotifyWebApi api, string id, CancellationToken cancellationToken) =>
        {
            return await api.GetAlbum(id, cancellationToken);
        });

        app.MapGet("/artists/{id}", async (ISpotifyWebApi api, string id, CancellationToken cancellationToken) =>
        {
            return await api.GetArtist(id, cancellationToken);
        });

        app.MapGet("/playlists/{id}", async (ISpotifyWebApi api, string id, CancellationToken cancellationToken) =>
        {
            return await api.GetPlaylist(id, cancellationToken);
        });
    }
}