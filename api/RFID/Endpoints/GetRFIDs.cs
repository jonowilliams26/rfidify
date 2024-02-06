namespace RFIDify.RFID.Endpoints;

public static class GetRFIDs
{
    public static void MapGetRFIDs(this IEndpointRouteBuilder app) => app
        .MapGet("/", Handle)
        .WithSummary("Get all RFID tags");

    private static async Task<Ok<List<RFIDTag>>> Handle(AppDbContext database, CancellationToken cancellationToken)
    {
        var rfids = await database.RFIDs
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return TypedResults.Ok(rfids);
    }
}
