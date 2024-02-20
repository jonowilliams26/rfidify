using System.Text.Json.Serialization;

namespace RFIDify.Spotify.Apis.WebApi.RequestResponse.Items;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Track), typeDiscriminator: nameof(SpotifyItemType.track))]
[JsonDerivedType(typeof(Album), typeDiscriminator: nameof(SpotifyItemType.album))]
[JsonDerivedType(typeof(Artist), typeDiscriminator: nameof(SpotifyItemType.artist))]
[JsonDerivedType(typeof(Playlist), typeDiscriminator: nameof(SpotifyItemType.playlist))]
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
    track,
}