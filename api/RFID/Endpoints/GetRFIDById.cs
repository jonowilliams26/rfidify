using System.Text.Json.Serialization;

namespace RFIDify.RFID.Endpoints;

public record GetRFIDByIdRequest(string Id);

public class GetRFIDByIdRequestValidator : AbstractValidator<GetRFIDByIdRequest>
{
    public GetRFIDByIdRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(GetRFIDByIdResponseTrack), typeDiscriminator: nameof(SpotifyItemType.Track))]
[JsonDerivedType(typeof(GetRFIDByIdResponseAlbum), typeDiscriminator: nameof(SpotifyItemType.Album))]
[JsonDerivedType(typeof(GetRFIDByIdResponseArtist), typeDiscriminator: nameof(SpotifyItemType.Artist))]
[JsonDerivedType(typeof(GetRFIDByIdResponsePlaylist), typeDiscriminator: nameof(SpotifyItemType.Playlist))]
public abstract record GetRFIDByIdResponse(string Id);
public record GetRFIDByIdResponseArtist(string Id, string Name, Uri? Image) : GetRFIDByIdResponse(Id);
public record GetRFIDByIdResponseAlbum(string Id, List<string> Artists, Uri? Image) : GetRFIDByIdResponse(Id);
public record GetRFIDByIdResponseTrack(string Id, string Name, List<string> Artists, Uri? Image) : GetRFIDByIdResponse(Id);
public record GetRFIDByIdResponsePlaylist(string Id, string Name, string? Description, Uri? Image) : GetRFIDByIdResponse(Id);

public static class GetRFIDById
{
    public static void MapGetRFIDById(this IEndpointRouteBuilder app) => app
        .MapGet("/{id}", Handle)
        .WithSummary("Get an RFID tag by its ID");

    private static async Task<Results<Ok<GetRFIDByIdResponse>, NotFound>> Handle([AsParameters] GetRFIDByIdRequest request, AppDbContext database, CancellationToken cancellationToken)
    {
        var rfid = await database.RFIDs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (rfid is null)
        {
            return TypedResults.NotFound();
        }

        GetRFIDByIdResponse response = rfid.SpotifyItem switch
        {
            SpotifyTrack track => new GetRFIDByIdResponseTrack(rfid.Id, track.Name, track.Artists.Select(x => x.Name).ToList(), track.Album.Images.FirstOrDefault()?.Url),
            SpotifyAlbum album => new GetRFIDByIdResponseAlbum(rfid.Id, album.Name, album.Artists.Select(x => x.Name).ToList(), album.Images.FirstOrDefault()?.Url),
            SpotifyArtist artist => new GetRFIDByIdResponseArtist(rfid.Id, artist.Name, artist.Images.FirstOrDefault()?.Url),
            SpotifyPlaylist playlist => new GetRFIDByIdResponsePlaylist(rfid.Id, playlist.Name, playlist.Description, playlist.Images.FirstOrDefault()?.Url),
            _ => throw new NotImplementedException()
        };

        return TypedResults.Ok(response);
    }
}
