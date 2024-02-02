namespace RFIDify.Spotify.Endpoints;

public static class GetTrack
{
    public static void MapGetTrack(this IEndpointRouteBuilder app) => app
        .MapGet("tracks/{id}", async (ISpotifyWebApi api, string id, CancellationToken cancellationToken) =>
        {
            var track = await api.GetTrack(id, cancellationToken);
            return TypedResults.Ok(track);
        });
}