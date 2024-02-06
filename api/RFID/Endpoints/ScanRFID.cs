using Microsoft.AspNetCore.SignalR;
using RFIDify.RFID.Hubs;

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

    private static async Task<Results<Ok, NotFound>> Handle(ScanRFIDRequest request, AppDbContext database, ILogger<ScanRFIDRequest> logger, ISpotifyWebApi api, IHubContext<RFIDHub, IRFIDHubClient> hubContext, CancellationToken cancellationToken)
    {
        await hubContext.Clients.All.RFIDScanned(new RFIDScannedMessage(request.Id));

        var rfid = await database.RFIDs
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (rfid is null)
        {
            return TypedResults.NotFound();
        }

        await api.Play(rfid.SpotifyItem, cancellationToken);
        return TypedResults.Ok();
    }
}