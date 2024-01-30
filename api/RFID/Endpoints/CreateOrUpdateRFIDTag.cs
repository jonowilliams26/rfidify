namespace RFIDify.RFID.Endpoints;

public record CreateOrUpdateRFIDTagRequest(string RFID, string SpotifyId);
public class CreateOrUpdateRFIDTagRequestValidator : AbstractValidator<CreateOrUpdateRFIDTagRequest>
{
    public CreateOrUpdateRFIDTagRequestValidator()
    {
        RuleFor(x => x.RFID).NotEmpty();
        RuleFor(x => x.SpotifyId)
            .NotEmpty()
            .Must(SpotifyId.IsValid)
            .WithMessage("Invalid Spotify ID");
    }
}

public static class CreateOrUpdateRFIDTag
{
    public static void MapCreateOrUpdateRFIDTag(this IEndpointRouteBuilder app) => app
        .MapPut("/", Handle)
        .WithSummary("Create or update an RFID tag")
        .WithRequestValidation<CreateOrUpdateRFIDTagRequest>();

    private static async Task<Results<Ok, Created>> Handle(CreateOrUpdateRFIDTagRequest request, AppDbContext database, ILogger<CreateOrUpdateRFIDTagRequest> logger, CancellationToken cancellationToken)
    {
        // TODO: Confirm spotify id exists

        var rfid = await database.RFIDs.SingleOrDefaultAsync(x => x.Id == request.RFID, cancellationToken);

        if (rfid is null)
        {
            logger.LogInformation("Creating RFID {RFID} - {SpotifyId}", request.RFID, request.SpotifyId);
            rfid = new RFIDTag
            {
                Id = request.RFID,
                SpotifyId = new SpotifyId(request.SpotifyId)
            };

            await database.RFIDs.AddAsync(rfid, cancellationToken);
            await database.SaveChangesAsync(cancellationToken);
            return TypedResults.Created();
        }

        logger.LogInformation("Updating RFID {RFID} to {SpotifyId}", request.RFID, request.SpotifyId);
        rfid.SpotifyId = new SpotifyId(request.SpotifyId);
        await database.SaveChangesAsync(cancellationToken);
        return TypedResults.Ok();
    }
}
