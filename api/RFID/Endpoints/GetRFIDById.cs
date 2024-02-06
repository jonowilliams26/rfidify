namespace RFIDify.RFID.Endpoints;

public record GetRFIDByIdRequest(string Id);
public class GetRFIDByIdRequestValidator : AbstractValidator<GetRFIDByIdRequest>
{
    public GetRFIDByIdRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public static class GetRFIDById
{
    public static void MapGetRFIDById(this IEndpointRouteBuilder app) => app
        .MapGet("/{id}", Handle)
        .WithSummary("Get an RFID tag by its ID");

    private static async Task<Results<Ok<RFIDTag>, NotFound>> Handle([AsParameters] GetRFIDByIdRequest request, AppDbContext database, CancellationToken cancellationToken)
    {
        var rfid = await database.RFIDs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (rfid is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(rfid);
    }
}
