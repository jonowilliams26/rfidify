namespace RFIDify.Spotify.Endpoints;

public record SearchRequest(string Search, SpotifyItemType Type);
public class SearchRequestValidator : AbstractValidator<SearchRequest>
{
    public SearchRequestValidator()
    {
        RuleFor(x => x.Search).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
    }
}

public static class Search
{
    public static void MapSearch(this IEndpointRouteBuilder app) => app
        .MapGet("/search", Handle)
        .WithSummary("Search for items on Spotify.");

    private static async Task<SpotifyPagedResponse<SpotifyItem>> Handle([AsParameters] SearchRequest request, ISpotifyWebApi api, CancellationToken cancellationToken)
    {
        return await api.Search(request.Search, request.Type, cancellationToken);
    }
}
