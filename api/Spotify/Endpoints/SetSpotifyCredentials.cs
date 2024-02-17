using RFIDify.Spotify.Apis.AccountsApi;

namespace RFIDify.Spotify.Endpoints;

public record SetSpotifyCredentialsRequest(string ClientId, string ClientSecret, string RedirectUri);
public record SetSpotifyCredentialsResponse(string AuthorizationUri);
public class SetSpotifyCredentialsRequestValidator : AbstractValidator<SetSpotifyCredentialsRequest>
{
    public SetSpotifyCredentialsRequestValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.ClientSecret).NotEmpty();
        RuleFor(x => x.RedirectUri)
            .NotEmpty()
            .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _))
            .WithMessage("Invalid URI");
    }
}

public static class SetSpotifyCredentials
{
    public static void MapSetSpotifyCredentials(this IEndpointRouteBuilder app) => app
        .MapPut("/credentials", Handle)
        .WithSummary("Sets the Spotify ClientId and Secret")
        .WithDescription("Returns the authorization URI to accept the Spotify terms and conditions")
        .WithRequestValidation<SetSpotifyCredentialsRequest>();

    private static async Task<Ok<SetSpotifyCredentialsResponse>> Handle(SetSpotifyCredentialsRequest request, AppDbContext database, ISpotifyAccountsApi api, CancellationToken cancellationToken)
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
        var response = new SetSpotifyCredentialsResponse(authorizationUri.ToString());
        return TypedResults.Ok(response);
    }
}