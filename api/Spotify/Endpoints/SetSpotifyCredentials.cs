using RFIDify.Spotify.Data;

namespace RFIDify.Spotify.Endpoints;

public static class SetSpotifyCredentials
{
    public static void MapSetSpotifyCredentials(this IEndpointRouteBuilder app) => app
        .MapPut("/credentials", Handle)
        .WithSummary("Sets the Spotify ClientId and Secret")
        .WithDescription("Returns the authorization URI to accept the Spotify terms and conditions");

    private record Request(string ClientId, string ClientSecret, string RedirectUri);
    private record Response(string AuthorizationUri);

    private static async Task<Ok<Response>> Handle(Request request, AppDbContext database, ISpotifyAccountsApi api, CancellationToken cancellationToken)
    {
        // Remove the old credentials
        var oldCredentials = await database.SpotifyCredentials.ToListAsync(cancellationToken);
        var oldAccessTokens = await database.SpotifyAccessToken.ToListAsync(cancellationToken);
        var oldRefreshTokens = await database.SpotifyRefreshToken.ToListAsync(cancellationToken);
        var oldAuthorizationStates = await database.SpotifyAuthorizationState.ToListAsync(cancellationToken);
        database.RemoveRange(oldCredentials);
        database.RemoveRange(oldAccessTokens);
        database.RemoveRange(oldRefreshTokens);
        database.RemoveRange(oldAuthorizationStates);

        // Add the new credentials
        var credentials = new SpotifyCredentials
        {
            ClientId = request.ClientId,
            ClientSecret = request.ClientSecret
        };

        var authorizationState = new SpotifyAuthorizationState
        {
            RedirectUri = request.RedirectUri
        };

        await database.SpotifyCredentials.AddAsync(credentials, cancellationToken);
        await database.SpotifyAuthorizationState.AddAsync(authorizationState, cancellationToken);
        await database.SaveChangesAsync(cancellationToken);

        // Generate the authorization URI
        var authorizationUri = api.GenerateAuthorizationUri(credentials, authorizationState);
        return TypedResults.Ok(new Response(authorizationUri.ToString()));
    }
}