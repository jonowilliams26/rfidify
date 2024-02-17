namespace RFIDify.Spotify.Apis.WebApi.RequestResponse.Items;

public record Image
{
    public required Uri Url { get; init; }
    public double? Width { get; init; }
    public double? Height { get; init; }
}