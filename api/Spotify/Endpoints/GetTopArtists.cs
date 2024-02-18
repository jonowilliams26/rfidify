using RFIDify.Spotify.Apis.WebApi;
using RFIDify.Spotify.Apis.WebApi.RequestResponse;
using RFIDify.Spotify.Apis.WebApi.RequestResponse.Items;

namespace RFIDify.Spotify.Endpoints;

public record GetTopArtistsRequest(int? Offset);
public class GetTopArtistsRequestValidator : AbstractValidator<GetTopArtistsRequest>
{
    public GetTopArtistsRequestValidator()
    {
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
    }
}

public static class GetTopArtists
{
    public static void MapGetTopArtists(this IEndpointRouteBuilder app) => app
        .MapGet("/artists", Handle)
        .WithSummary("Get the current user's top artists.");

    private static async Task<PagedResponse<Artist>> Handle([AsParameters] GetTopArtistsRequest request, ISpotifyWebApi api, CancellationToken cancellationToken)
    {
        return await api.GetTopArtists(request.Offset, cancellationToken);
    }
}
