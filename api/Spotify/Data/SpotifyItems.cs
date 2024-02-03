using System.Text.Json.Serialization;

namespace RFIDify.Spotify.Data;

[JsonDerivedType(typeof(SpotifyTrack), typeDiscriminator: nameof(SpotifyItemType.Track))]
[JsonDerivedType(typeof(SpotifyAlbum), typeDiscriminator: nameof(SpotifyItemType.Album))]
[JsonDerivedType(typeof(SpotifyArtist), typeDiscriminator: nameof(SpotifyItemType.Artist))]
[JsonDerivedType(typeof(SpotifyPlaylist), typeDiscriminator: nameof(SpotifyItemType.Playlist))]
public interface ISpotifyItem
{
    SpotifyItemType Type { get; }
    string Id { get; }
    string Uri { get; }
    string Name { get; }
}

public enum SpotifyItemType
{
    Album,
    Artist,
    Playlist,
    Track
}

public record SpotifyTrack : ISpotifyItem
{
    public SpotifyItemType Type => SpotifyItemType.Track;
    public required string Id { get; init; }
    public required string Uri { get; init; }
    public required string Name { get; init; }
    public required SpotifyAlbum Album { get; init; }
    public List<SpotifyArtist> Artists { get; init; } = [];
}

public record SpotifyAlbum : ISpotifyItem
{
    public SpotifyItemType Type => SpotifyItemType.Album;
    public required string Id { get; init; }
    public required string Uri { get; init; }
    public required string Name { get; init; }
    public required string AlbumType { get; init; }
    public List<SpotifyArtist> Artists { get; init; } = [];
    public List<SpotifyImage> Images { get; init; } = [];
}

public record SpotifyArtist : ISpotifyItem
{
    public SpotifyItemType Type => SpotifyItemType.Artist;
    public required string Id { get; init; }
    public required string Uri { get; init; }
    public required string Name { get; init; }
    public List<SpotifyImage> Images { get; init; } = [];
}

public record SpotifyPlaylist : ISpotifyItem
{
    public SpotifyItemType Type => SpotifyItemType.Playlist;
    public required string Id { get; init; }
    public required string Uri { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public List<SpotifyImage> Images { get; init; } = [];
}

public record SpotifyImage
{
    public required string Url { get; init; }
    public double? Width { get; init; }
    public double? Height { get; init; }
}