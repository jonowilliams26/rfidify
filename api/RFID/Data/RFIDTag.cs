﻿namespace RFIDify.RFID.Data;

public class RFIDTag
{
    public required string Id { get; init; }
    public required SpotifyItem SpotifyItem { get; set; }
}