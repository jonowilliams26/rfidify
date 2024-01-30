using RFIDify.Spotify.Endpoints;

namespace RFIDify.Api;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup(string.Empty)
            .WithOpenApi();

        endpoints.MapSpotifyEndpoints();
    }

    private static void MapSpotifyEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/spotify")
            .WithTags("Spotify");

        endpoints.MapSetSpotifyCredentials();
    }
}
