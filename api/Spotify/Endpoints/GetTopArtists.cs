namespace RFIDify.Spotify.Endpoints;

public record GetTopArtistsRequest(int? Offset);

public static class GetTopArtists
{
    public static void MapGetTopArtists(this IEndpointRouteBuilder app) => app
        .MapGet("/artists", Handle)
        .WithSummary("Get the current user's top artists.");

    private static async Task<SpotifyPagedResponse<SpotifyArtist>> Handle([AsParameters] GetTopArtistsRequest request, ISpotifyWebApi api, CancellationToken cancellationToken)
    {
        return await api.GetTopArtists(request.Offset, cancellationToken);
    }
}
