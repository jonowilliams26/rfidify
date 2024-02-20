using RFIDify.Spotify.Apis.WebApi.RequestResponse.Items;

namespace RFIDify.Spotify.Apis.WebApi.RequestResponse;

public record GetAlbumsRequest(int? Offset) : ISpotifyRequest
{
    public string Uri()
    {
        var query = new Dictionary<string, string?>
        {
            [QueryParams.Offset] = Offset?.ToString() ?? "0"
        };

        return $"me/albums{QueryString.Create(query)}";
    }
}

public record GetSavedAlbumsResponseItem
{
    public required Album Album { get; init; }
}