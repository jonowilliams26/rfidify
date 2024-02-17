namespace RFIDify.Spotify.Apis;

public interface ISpotifyRequest
{
    string Uri();
}

public interface ISpotifyRequestFormUrlEncodeable : ISpotifyRequest
{
    Dictionary<string, string> FormContent();
}