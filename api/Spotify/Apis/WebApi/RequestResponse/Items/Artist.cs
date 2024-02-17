namespace RFIDify.Spotify.Apis.WebApi.RequestResponse.Items;

public record Artist : SpotifyItem
{
    public override SpotifyItemType Type => SpotifyItemType.artist;
    public List<Image> Images { get; init; } = [];
}

public record ArtistNoImages : SpotifyItem
{
    public override SpotifyItemType Type => SpotifyItemType.artist;
}