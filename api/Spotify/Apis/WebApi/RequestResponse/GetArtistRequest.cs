namespace RFIDify.Spotify.Apis.WebApi.RequestResponse;

public record GetArtistRequest(string Id) : ISpotifyRequest
{
    public string Uri() => $"artists/{Id}";
}
