using RFIDify.RFID.Endpoints;
using RFIDify.Spotify.Endpoints;

namespace RFIDify.Api;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup(string.Empty)
            .WithOpenApi();

        endpoints.MapSpotifyEndpoints();
        endpoints.MapRFIDEndpoints();
    }

    private static void MapSpotifyEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/spotify")
            .WithTags("Spotify");

        endpoints.MapIsSpotifyCredentialsSet();
        endpoints.MapSetSpotifyCredentials();
        endpoints.MapAuthorizationCallback();
        endpoints.MapGetTopTracks();
        endpoints.MapGetTopArtists();
        endpoints.MapGetPlaylists();
        endpoints.MapSpotifyTestEndpoints();
    }

    private static void MapRFIDEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/rfids")
            .WithTags("RFID");

        endpoints.MapGetRFIDById();
        endpoints.MapGetRFIDs();
        endpoints.MapCreateOrUpdateRFIDTag();
        endpoints.MapScanRFID();
        endpoints.MapDeleteRFID();
    }
}
