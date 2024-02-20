using RFIDify.Spotify.Apis.WebApi.RequestResponse.Items;

namespace RFIDify.Spotify.Apis.WebApi.RequestResponse;

public record GetCurrentlyPlayingRequest : ISpotifyRequest
{
    public string Uri() => "me/player/currently-playing";
}

public record GetCurrentlyPlayingResponse(int? ProgressMs, Track? Item);
