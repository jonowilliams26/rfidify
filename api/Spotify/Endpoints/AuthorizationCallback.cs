namespace RFIDify.Spotify.Endpoints;

public record AuthorizationCallbackRequest(string? Code, string? State, string? Error);
public class AuthorizationCallbackRequestValidator : AbstractValidator<AuthorizationCallbackRequest>
{
    public AuthorizationCallbackRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .When(x => x.Error is null);

        RuleFor(x => x.State).NotEmpty();

        RuleFor(x => x.Error)
            .NotEmpty()
            .When(x => x.Code is null);
    }
}

public static class AuthorizationCallback
{
    public static void MapAuthorizationCallback(this IEndpointRouteBuilder app) => app
        .MapGet("/authorize", Handle)
        .WithSummary("The authorization callback which is trigger when the user accepts the Spotify terms and conditions")
        .WithDescription("Exchanges the authorization code for an access token and refresh token which will be used to make request to the Spotify Web API")
        .WithRequestValidation<AuthorizationCallbackRequest>();

    private static async Task<Results<Ok, UnauthorizedHttpResult, ValidationProblem>> Handle([AsParameters] AuthorizationCallbackRequest request, AppDbContext database, ISpotifyAccountsApi api, CancellationToken cancellationToken)
    {
        if (request.Error is not null)
        {
            var errors = new Dictionary<string, string[]> { ["Error"] = [request.Error] };
            return TypedResults.ValidationProblem(errors, "An error occurred while authorizing with Spotify");
        }

        var authorizationState = await database.SpotifyAuthorizationState.SingleOrDefaultAsync(x => x.State == request.State, cancellationToken);
        if (authorizationState is null)
        {
            return TypedResults.Unauthorized();
        }

        var tokens = await api.ExchangeAuthorizationCodeForTokens(request.Code!, authorizationState, cancellationToken);

        await database.SpotifyAccessToken.AddAsync(tokens.AccessToken, cancellationToken);
        await database.SpotifyRefreshToken.AddAsync(tokens.RefreshToken, cancellationToken);
        database.Remove(authorizationState);
        await database.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok();
    }
}