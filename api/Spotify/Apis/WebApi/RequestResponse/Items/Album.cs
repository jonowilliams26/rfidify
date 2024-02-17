namespace RFIDify.Spotify.Apis.WebApi.RequestResponse.Items;

public record Album : SpotifyItem
{
    public override SpotifyItemType Type => SpotifyItemType.album;
    public List<ArtistNoImages> Artists { get; init; } = [];
    public List<Image> Images { get; init; } = [];
}
