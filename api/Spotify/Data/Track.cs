namespace RFIDify.Spotify.Data;

public class Track
{
    public required string Id { get; init; }
    public required string Uri { get; init; }
    public required string Name { get; init; }
    public required Album Album { get; init; }
    public List<Artist> Artists { get; init; } = [];
}

public class Artist
{
    public required string Id { get; init; }
    public required string Uri { get; init; }
    public required string Name { get; init; }
    public List<Image> Images { get; init; } = [];
}

public class Album
{
    public required string Id { get; init; }
    public required string Uri { get; init; }
    public required string Name { get; init; }
    public List<Artist> Artists { get; init; } = [];
    public List<Image> Images { get; init; } = [];
}

public class Playlist
{
    public required string Id { get; init; }
    public required string Uri { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public List<Image> Images { get; init; } = [];
}

public class Image
{
    public required string Url { get; init; }
    public int? Height { get; init; }
    public int? Width { get; init; }
}