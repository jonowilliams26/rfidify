namespace RFIDify.Spotify.Apis.WebApi.RequestResponse;

public record GetAlbumRequest(string Id) : ISpotifyRequest
{
    public string Uri() => $"albums/{Id}";
}
