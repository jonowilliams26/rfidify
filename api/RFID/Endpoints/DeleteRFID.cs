namespace RFIDify.RFID.Endpoints;

public static class DeleteRFID
{
    public static void MapDeleteRFID(this IEndpointRouteBuilder app) => app
        .MapDelete("/{id}", Handle)
        .WithSummary("Delete an RFID");

    private static async Task<Results<NoContent, NotFound>> Handle(string id, AppDbContext database, CancellationToken cancellationToken)
    {
        var rfid = await database.RFIDs.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (rfid is null)
        {
            return TypedResults.NotFound();
        }

        database.RFIDs.Remove(rfid);
        await database.SaveChangesAsync(cancellationToken);
        return TypedResults.NoContent();
    }
}
