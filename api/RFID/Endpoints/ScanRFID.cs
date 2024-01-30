namespace RFIDify.RFID.Endpoints;

public record ScanRFIDRequest(string Id);
public class ScanRFIDRequestValidator : AbstractValidator<ScanRFIDRequest>
{
    public ScanRFIDRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public static class ScanRFID
{
    public static void MapScanRFID(this IEndpointRouteBuilder app) => app
        .MapPost("/scan", Handle)
        .WithSummary("Scan an RFID tag and start playing on Spotify")
        .WithRequestValidation<ScanRFIDRequest>();

    private static async Task<Results<Ok, NotFound>> Handle(ScanRFIDRequest request, AppDbContext database, ILogger<ScanRFIDRequest> logger, ISpotifyWebApi api, CancellationToken cancellationToken)
    {
        var rfid = await database.RFIDs
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (rfid is null)
        {
            return TypedResults.NotFound();
        }

        await api.Play(rfid.SpotifyId, cancellationToken);
        return TypedResults.Ok();
    }
}