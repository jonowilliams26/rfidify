using System.Text.Json.Serialization;

namespace RFIDify.Spotify.Data;

[JsonDerivedType(typeof(SpotifyTrack), typeDiscriminator: nameof(SpotifyItemType.Track))]
[JsonDerivedType(typeof(SpotifyAlbum), typeDiscriminator: nameof(SpotifyItemType.Album))]
[JsonDerivedType(typeof(SpotifyArtist), typeDiscriminator: nameof(SpotifyItemType.Artist))]
[JsonDerivedType(typeof(SpotifyPlaylist), typeDiscriminator: nameof(SpotifyItemType.Playlist))]
public abstract record SpotifyItem
{
    public abstract SpotifyItemType Type { get; }
    public required string Id { get; init; }
    public required string Uri { get; init; }
    public required string Name { get; init; }
}

public enum SpotifyItemType
{
    Album,
    Artist,
    Playlist,
    Track
}

public record SpotifyTrack : SpotifyItem
{
    public override SpotifyItemType Type => SpotifyItemType.Track;
    public required SpotifyAlbum Album { get; init; }
    public List<SpotifyArtist> Artists { get; init; } = [];
}

public record SpotifyAlbum : SpotifyItem
{
    public override SpotifyItemType Type => SpotifyItemType.Album;
    public List<SpotifyArtist> Artists { get; init; } = [];
    public List<SpotifyImage> Images { get; init; } = [];
}

public record SpotifyArtist : SpotifyItem
{
    public override SpotifyItemType Type => SpotifyItemType.Artist;
    public List<SpotifyImage> Images { get; init; } = [];
}

public record SpotifyPlaylist : SpotifyItem
{
    public override SpotifyItemType Type => SpotifyItemType.Playlist;
    public string? Description { get; init; }
    public List<SpotifyImage> Images { get; init; } = [];
}

public record SpotifyImage
{
    public required Uri Url { get; init; }
    public double? Width { get; init; }
    public double? Height { get; init; }
}