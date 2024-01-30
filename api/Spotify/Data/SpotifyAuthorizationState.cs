using System.Security.Cryptography;

namespace RFIDify.Spotify.Data;

public class SpotifyAuthorizationState
{
    public required string RedirectUri { get; set; }
    public string State { get; private init; } = RandomNumberGenerator.GetHexString(32);
}
