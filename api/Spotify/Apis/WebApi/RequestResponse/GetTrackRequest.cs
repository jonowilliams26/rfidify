namespace RFIDify.Spotify.Apis.WebApi.RequestResponse;

public record GetTrackRequest(string Id) : ISpotifyRequest
{
    public string Uri() => $"tracks/{Id}";
}
