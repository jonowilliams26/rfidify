using RFIDify.Spotify.Apis.WebApi;
using RFIDify.Spotify.Apis.WebApi.RequestResponse;
using RFIDify.Spotify.Apis.WebApi.RequestResponse.Items;

namespace RFIDify.Spotify.Endpoints;

public record SearchRequest(string Search, SpotifyItemType Type, int? Offset);
public class SearchRequestValidator : AbstractValidator<SearchRequest>
{
    public SearchRequestValidator()
    {
        RuleFor(x => x.Search).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
    }
}

public static class Search
{
    public static void MapSearch(this IEndpointRouteBuilder app) => app
        .MapGet("/search", Handle)
        .WithSummary("Search for items on Spotify.")
        .WithRequestValidation<SearchRequest>();

    private static async Task<PagedResponse<SpotifyItem>> Handle([AsParameters] SearchRequest request, ISpotifyWebApi api, CancellationToken cancellationToken)
    {
        return await api.Search(request.Search, request.Type, request.Offset, cancellationToken);
    }
}
