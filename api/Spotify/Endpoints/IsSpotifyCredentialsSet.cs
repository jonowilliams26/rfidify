namespace RFIDify.Spotify.Endpoints;

public static class IsSpotifyCredentialsSet
{
    public static void MapIsSpotifyCredentialsSet(this IEndpointRouteBuilder app) => app
        .MapGet("/credentials", Handle)
        .WithSummary("Confirms if the Spotify credentials are set");

    private static async Task<Results<Ok, NotFound>> Handle(AppDbContext database, CancellationToken cancellationToken)
    {
        var credentials = await database.SpotifyCredentials
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        if (credentials is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok();
    }
}
