namespace RFIDify.Spotify.Apis.WebApi.RequestResponse;

public record PlayRequest : ISpotifyRequest
{
    public string? ContextUri { get; init; }
    public string[]? Uris { get; init; }

    public PlayRequest(SpotifyItem item)
    {
        var isTrack = item.Type is SpotifyItemType.track;
        ContextUri = isTrack ? null : item.Uri;
        Uris = isTrack ? [item.Uri] : null;
    }

    public string Uri() => "me/player/play";
}