using System.Text.Json.Serialization;

namespace RFIDify.RFID.Endpoints;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(GetRFIDsResponseTrack), typeDiscriminator: nameof(SpotifyItemType.Track))]
[JsonDerivedType(typeof(GetRFIDsResponseAlbum), typeDiscriminator: nameof(SpotifyItemType.Album))]
[JsonDerivedType(typeof(GetRFIDsResponseArtist), typeDiscriminator: nameof(SpotifyItemType.Artist))]
[JsonDerivedType(typeof(GetRFIDsResponsePlaylist), typeDiscriminator: nameof(SpotifyItemType.Playlist))]
public abstract record GetRFIDsResponseItem(string Id);
public record GetRFIDsResponseArtist(string Id, string Name, Uri? Image) : GetRFIDsResponseItem(Id);
public record GetRFIDsResponseAlbum(string Id, string Name, List<string> Artists, Uri? Image) : GetRFIDsResponseItem(Id);
public record GetRFIDsResponseTrack(string Id, string Name, List<string> Artists, Uri? Image) : GetRFIDsResponseItem(Id);
public record GetRFIDsResponsePlaylist(string Id, string Name, string? Description, Uri? Image) : GetRFIDsResponseItem(Id);

public static class GetRFIDs
{
    public static void MapGetRFIDs(this IEndpointRouteBuilder app) => app
        .MapGet("/", Handle)
        .WithSummary("Get all RFID tags");

    private static async Task<Ok<List<GetRFIDsResponseItem>>> Handle(AppDbContext database, CancellationToken cancellationToken)
    {
        var rfids = await database.RFIDs
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var response = rfids.Select<RFIDTag, GetRFIDsResponseItem>(x => x.SpotifyItem switch
        {
            SpotifyTrack track => new GetRFIDsResponseTrack(x.Id, track.Name, track.Artists.Select(x => x.Name).ToList(), track.Album.Images.FirstOrDefault()?.Url),
            SpotifyAlbum album => new GetRFIDsResponseAlbum(x.Id, album.Name, album.Artists.Select(x => x.Name).ToList(), album.Images.FirstOrDefault()?.Url),
            SpotifyArtist artist => new GetRFIDsResponseArtist(x.Id, artist.Name, artist.Images.FirstOrDefault()?.Url),
            SpotifyPlaylist playlist => new GetRFIDsResponsePlaylist(x.Id, playlist.Name, playlist.Description, playlist.Images.FirstOrDefault()?.Url),
            _ => throw new NotImplementedException()
        }).ToList();

        return TypedResults.Ok(response);
    }
}
