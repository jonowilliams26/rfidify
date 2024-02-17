namespace RFIDify.Spotify.Apis.AccountsApi.RequestResponse;

public static class FormParams
{
    public const string ClientId = "client_id";
    public const string ResponseType = "response_type";
    public const string RedirectUri = "redirect_uri";
    public const string State = "state";
    public const string Scope = "scope";
    public const string ShowDialog = "show_dialog";
    public const string Code = "code";
    public const string GrantType = "grant_type";
    public const string RefreshToken = "refresh_token";
}

public static class ResponseTypes
{
    public const string Code = "code";
}

public static class GrantTypes
{
    public const string AuthorizationCode = "authorization_code";
    public const string RefreshToken = "refresh_token";
}
