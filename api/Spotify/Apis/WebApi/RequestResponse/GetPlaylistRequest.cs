namespace RFIDify.Spotify.Apis.WebApi.RequestResponse;

public record GetPlaylistRequest(string Id) : ISpotifyRequest
{
    public string Uri()
    {
        var query = new Dictionary<string, string?>
        {
            [QueryParams.Fields] = Fields.All,
        };

        return $"playlists/{Id}{QueryString.Create(query)}";
    }
}