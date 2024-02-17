namespace RFIDify.Spotify.Apis.WebApi.RequestResponse;

public static class QueryParams
{
    public const string Search = "q";
    public const string Type = "type";
    public const string Offset = "offset";
    public const string TimeRange = "time_range";
    public const string Fields = "fields";
}

public static class TimeRanges
{
    public const string ShortTerm = "short_term";
}

public static class Fields
{
    public const string All = $"{Id},{Uri},{Name},{Description},{Images}";
    public const string Id = "id";
    public const string Uri = "uri";
    public const string Name = "name";
    public const string Description = "description";
    public const string Images = "images";
}