namespace RFIDify.Spotify.Apis.WebApi.RequestResponse.Items;

public record Playlist : SpotifyItem
{
    public override SpotifyItemType Type => SpotifyItemType.playlist;
    public string? Description { get; set; }
    public List<Image> Images { get; init; } = [];
}
