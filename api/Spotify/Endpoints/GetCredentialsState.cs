namespace RFIDify.Spotify.Endpoints;

public static class GetCredentialsState
{
    public static void MapGetCredentialsState(this IEndpointRouteBuilder app) => app
        .MapGet("/", Handle)
        .WithSummary("Get the state of the Spotify credentials");

    private static async Task<Results<Ok, UnauthorizedHttpResult>> Handle(AppDbContext database, CancellationToken cancellationToken)
    {
        var credentials = await database.SpotifyCredentials
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (credentials is null)
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Ok();
    }
}
