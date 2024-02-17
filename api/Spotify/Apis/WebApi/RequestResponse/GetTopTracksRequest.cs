namespace RFIDify.Spotify.Apis.WebApi.RequestResponse;

public record GetTopTracksRequest(int? Offset) : ISpotifyRequest
{
    public string Uri()
    {
        var query = new Dictionary<string, string?>
        {
            [QueryParams.Offset] = Offset?.ToString() ?? "0",
            [QueryParams.TimeRange] = TimeRanges.ShortTerm
        };
        return $"me/top/tracks{QueryString.Create(query)}";
    }
}