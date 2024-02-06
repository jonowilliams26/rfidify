using RFIDify.RFID.Endpoints.Dtos;

namespace RFIDify.RFID.Endpoints;

public static class GetRFIDs
{
    public static void MapGetRFIDs(this IEndpointRouteBuilder app) => app
        .MapGet("/", Handle)
        .WithSummary("Get all RFID tags");

    private static async Task<Ok<List<RFIDDto>>> Handle(AppDbContext database, CancellationToken cancellationToken)
    {
        var rfids = await database.RFIDs
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var response = rfids.Select(RFIDDto.Create).ToList();

        return TypedResults.Ok(response);
    }
}
