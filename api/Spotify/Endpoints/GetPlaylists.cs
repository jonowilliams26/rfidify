using RFIDify.Spotify.Apis.WebApi;

namespace RFIDify.Spotify.Endpoints;

public record GetPlaylistsRequest(int? Offset);

public static class GetPlaylists
{
    public static void MapGetPlaylists(this IEndpointRouteBuilder app) => app
        .MapGet("/playlists", Handle)
        .WithSummary("Get the current user's playlists.");

    private static async Task<SpotifyPagedResponse<SpotifyPlaylist>> Handle([AsParameters] GetPlaylistsRequest request, ISpotifyWebApi api, CancellationToken cancellationToken)
    {
        return await api.GetPlaylists(request.Offset, cancellationToken);
    }
}
