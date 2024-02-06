using RFIDify.RFID.Endpoints.Dtos;
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

public static class GetRFIDById
{
    public static void MapGetRFIDById(this IEndpointRouteBuilder app) => app
        .MapGet("/{id}", Handle)
        .WithSummary("Get an RFID tag by its ID");

    private static async Task<Results<Ok<RFIDDto>, NotFound>> Handle([AsParameters] GetRFIDByIdRequest request, AppDbContext database, CancellationToken cancellationToken)
    {
        var rfid = await database.RFIDs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (rfid is null)
        {
            return TypedResults.NotFound();
        }

        var response = RFIDDto.Create(rfid);

        return TypedResults.Ok(response);
    }
}
