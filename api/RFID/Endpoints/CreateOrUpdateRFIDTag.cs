namespace RFIDify.RFID.Endpoints;

public record CreateOrUpdateRFIDTagRequest(string RFID, string SpotifyUri);
public class CreateOrUpdateRFIDTagRequestValidator : AbstractValidator<CreateOrUpdateRFIDTagRequest>
{
    public CreateOrUpdateRFIDTagRequestValidator()
    {
        RuleFor(x => x.RFID).NotEmpty();
        RuleFor(x => x.SpotifyUri)
            .NotEmpty()
            .Must(SpotifyUri.IsValid)
            .WithMessage("Invalid Spotify Uri");
    }
}

public static class CreateOrUpdateRFIDTag
{
    public static void MapCreateOrUpdateRFIDTag(this IEndpointRouteBuilder app) => app
        .MapPut("/", Handle)
        .WithSummary("Create or update an RFID tag")
        .WithRequestValidation<CreateOrUpdateRFIDTagRequest>();

    private static async Task<Results<Ok, Created>> Handle(CreateOrUpdateRFIDTagRequest request, AppDbContext database, ILogger<CreateOrUpdateRFIDTagRequest> logger, ISpotifyWebApi api, CancellationToken cancellationToken)
    {
        var uri = new SpotifyUri(request.SpotifyUri);
        var item = await api.Get(uri, cancellationToken);

        var rfid = await database.RFIDs.SingleOrDefaultAsync(x => x.Id == request.RFID, cancellationToken);
        if (rfid is null)
        {
            rfid = new RFIDTag 
            { 
                Id = request.RFID,
                SpotifyItem = item
            };
            await database.RFIDs.AddAsync(rfid, cancellationToken);
            await database.SaveChangesAsync(cancellationToken);
            return TypedResults.Created();
        }

        rfid.SpotifyItem = item;
        await database.SaveChangesAsync(cancellationToken);
        return TypedResults.Ok();
    }
}
