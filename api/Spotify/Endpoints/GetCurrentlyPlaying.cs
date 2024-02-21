using RFIDify.Spotify.Apis.WebApi;

namespace RFIDify.Spotify.Endpoints;

public record GetCurrentlyPlayingResponse(string Name, string Artists, string Progress);

public static class GetCurrentlyPlaying
{
    public static void MapGetCurrentlyPlaying(this IEndpointRouteBuilder app) => app
        .MapGet("/currently-playing", Handle)
        .WithSummary("Get the currently playing Spotify item");

    private static async Task<Results<Ok<GetCurrentlyPlayingResponse>, NotFound>> Handle(ISpotifyWebApi api, CancellationToken cancellationToken)
    {
        var apiResponse = await api.GetCurrentlyPlaying(cancellationToken);
        if (apiResponse?.Item is null)
        {
            return TypedResults.NotFound();
        }

        var name = apiResponse.Item.Name;
        var artists = string.Join(", ", apiResponse.Item.Artists.Select(x => x.Name));
        var progress = TimeSpan.FromMilliseconds(apiResponse.ProgressMs ?? 0).ToString(@"mm\:ss");

        var response = new GetCurrentlyPlayingResponse(name, artists, progress);
        return TypedResults.Ok(response);
    }
}
