using RFIDify.Spotify.Apis.WebApi;
using RFIDify.Spotify.Apis.WebApi.RequestResponse;
using RFIDify.Spotify.Apis.WebApi.RequestResponse.Items;

namespace RFIDify.Spotify.Endpoints;

public record GetTopTracksRequest(int? Offset);
public class GetTopTracksRequestValidator : AbstractValidator<GetTopTracksRequest>
{
    public GetTopTracksRequestValidator()
    {
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
    }
}

public static class GetTopTracks
{
    public static void MapGetTopTracks(this IEndpointRouteBuilder app) => app
        .MapGet("/tracks", Handle)
        .WithSummary("Get the current user's top tracks.")
        .WithRequestValidation<GetTopTracksRequest>();

    private static async Task<PagedResponse<Track>> Handle([AsParameters] GetTopTracksRequest request, ISpotifyWebApi api, CancellationToken cancellationToken)
    {
        return await api.GetTopTracks(request.Offset, cancellationToken);
    }
}