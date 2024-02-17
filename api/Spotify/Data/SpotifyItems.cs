using System.Text.Json.Serialization;

namespace RFIDify.Spotify.Data;

[JsonDerivedType(typeof(SpotifyTrack), typeDiscriminator: nameof(SpotifyItemType.track))]
[JsonDerivedType(typeof(SpotifyAlbum), typeDiscriminator: nameof(SpotifyItemType.album))]
[JsonDerivedType(typeof(SpotifyArtist), typeDiscriminator: nameof(SpotifyItemType.artist))]
[JsonDerivedType(typeof(SpotifyPlaylist), typeDiscriminator: nameof(SpotifyItemType.playlist))]
public abstract record SpotifyItem
{
    public abstract SpotifyItemType Type { get; }
    public required string Id { get; init; }
    public required string Uri { get; init; }
    public required string Name { get; init; }
}

public enum SpotifyItemType
{
    album,
    artist,
    playlist,
    track
}

public record SpotifyTrack : SpotifyItem
{
    public override SpotifyItemType Type => SpotifyItemType.track;
    public required SpotifyAlbum Album { get; init; }
    public List<SpotifyArtist> Artists { get; init; } = [];
}

public record SpotifyAlbum : SpotifyItem
{
    public override SpotifyItemType Type => SpotifyItemType.album;
    public List<SpotifyArtist> Artists { get; init; } = [];
    public List<SpotifyImage> Images { get; init; } = [];
}

public record SpotifyArtist : SpotifyItem
{
    public override SpotifyItemType Type => SpotifyItemType.artist;
    public List<SpotifyImage> Images { get; init; } = [];
}

public record SpotifyPlaylist : SpotifyItem
{
    public override SpotifyItemType Type => SpotifyItemType.playlist;
    public string? Description { get; set; }
    public List<SpotifyImage> Images { get; init; } = [];
}

public record SpotifyImage
{
    public required Uri Url { get; init; }
    public double? Width { get; init; }
    public double? Height { get; init; }
}