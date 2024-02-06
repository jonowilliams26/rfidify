using System.Text.Json.Serialization;

namespace RFIDify.RFID.Endpoints.Dtos;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(RFIDTrackDto), typeDiscriminator: nameof(SpotifyItemType.Track))]
[JsonDerivedType(typeof(RFIDAlbumDto), typeDiscriminator: nameof(SpotifyItemType.Album))]
[JsonDerivedType(typeof(RFIDArtistDto), typeDiscriminator: nameof(SpotifyItemType.Artist))]
[JsonDerivedType(typeof(RFIDPlaylistDto), typeDiscriminator: nameof(SpotifyItemType.Playlist))]
public abstract record RFIDDto(string Id)
{
    public static RFIDDto Create(RFIDTag rfid) => rfid.SpotifyItem switch
    {
        SpotifyTrack track => new RFIDTrackDto(rfid.Id, track.Name, track.Artists.Select(x => x.Name).ToList(), track.Album.Images.FirstOrDefault()?.Url),
        SpotifyAlbum album => new RFIDAlbumDto(rfid.Id, album.Name, album.Artists.Select(x => x.Name).ToList(), album.Images.FirstOrDefault()?.Url),
        SpotifyArtist artist => new RFIDArtistDto(rfid.Id, artist.Name, artist.Images.FirstOrDefault()?.Url),
        SpotifyPlaylist playlist => new RFIDPlaylistDto(rfid.Id, playlist.Name, playlist.Description, playlist.Images.FirstOrDefault()?.Url),
        _ => throw new NotImplementedException()
    };
}
public record RFIDArtistDto(string Id, string Name, Uri? Image) : RFIDDto(Id);
public record RFIDAlbumDto(string Id, string Name, List<string> Artists, Uri? Image) : RFIDDto(Id);
public record RFIDTrackDto(string Id, string Name, List<string> Artists, Uri? Image) : RFIDDto(Id);
public record RFIDPlaylistDto(string Id, string Name, string? Description, Uri? Image) : RFIDDto(Id);