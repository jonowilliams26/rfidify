using RFIDify.Spotify.Apis.WebApi;

namespace RFIDify.Spotify.Endpoints;

public record GetTopTracksRequest(int? Offset);

public static class GetTopTracks
{
    public static void MapGetTopTracks(this IEndpointRouteBuilder app) => app
        .MapGet("/tracks", Handle)
        .WithSummary("Get the current user's top tracks.");

    private static async Task<SpotifyPagedResponse<SpotifyTrack>> Handle([AsParameters] GetTopTracksRequest request, ISpotifyWebApi api, CancellationToken cancellationToken)
    {
        return await api.GetTopTracks(request.Offset, cancellationToken);
    }
}