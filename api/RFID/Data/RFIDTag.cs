namespace RFIDify.RFID.Data;

public class RFIDTag
{
    public required string Id { get; init; }
    public required ISpotifyItem SpotifyItem { get; set; }
}