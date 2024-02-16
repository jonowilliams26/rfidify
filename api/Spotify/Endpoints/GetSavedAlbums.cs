namespace RFIDify.Spotify.Endpoints;

public record GetSavedAlbumsRequest(int? Offset);

public static class GetSavedAlbums
{
    public static void MapGetSavedAlbums(this IEndpointRouteBuilder app) => app
        .MapGet("/albums", Handle)
        .WithSummary("Get the current user's saved albums.");

    private static async Task<SpotifyPagedResponse<SpotifyAlbum>> Handle([AsParameters] GetSavedAlbumsRequest request, ISpotifyWebApi api, CancellationToken cancellationToken)
    {
        return await api.GetSavedAlbums(request.Offset, cancellationToken);
    }
}