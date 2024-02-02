using System.Text.RegularExpressions;

namespace RFIDify.Spotify.Data;

public class SpotifyMetadata
{
    public required string Id { get; init; }
    public required SpotifyType Type { get; init; }
    public required string Name { get; set; }
    public Uri? Image { get; set; }
    public string? Description { get; set; }
    public List<string> Artists { get; set; } = [];
}

public partial record SpotifyId
{
    public string Id { get; init; }
    public SpotifyIdType Type { get; init; }

    public SpotifyId(string id)
    {
        Id = id;

        if (!IsValid(id))
        {
            throw new ArgumentException("Invalid Spotify ID", nameof(id));
        }

        var type = id.Split(':')[1];
        Type = type switch
        {
            "album" => SpotifyIdType.Album,
            "artist" => SpotifyIdType.Artist,
            "playlist" => SpotifyIdType.Playlist,
            "track" => SpotifyIdType.Track,
            _ => throw new ArgumentException("Invalid Spotify ID", nameof(id))
        };
    }

    [GeneratedRegex(@"spotify:(playlist|album|track|artist):[a-zA-Z0-9]+")]
    private static partial Regex SpotifyIdRegex();
    public static bool IsValid(string id) => SpotifyIdRegex().IsMatch(id);
}

public enum SpotifyType
{
    Album,
    Artist,
    Playlist,
    Track
}
