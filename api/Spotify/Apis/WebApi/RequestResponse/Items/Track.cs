namespace RFIDify.Spotify.Apis.WebApi.RequestResponse.Items;

public record Track : SpotifyItem
{
    public override SpotifyItemType Type => SpotifyItemType.track;
    public required Album Album { get; init; }
    public List<ArtistNoImages> Artists { get; init; } = [];
}
