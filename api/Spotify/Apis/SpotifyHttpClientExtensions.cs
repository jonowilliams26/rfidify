using RFIDify.Spotify.Apis.WebApi;
using System.Runtime.Serialization;
using System.Text.Json;

namespace RFIDify.Spotify.Apis;

public static class SpotifyHttpClientExtensions
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = SpotifyWebApi.JsonSerializerOptions;

    public static async Task<HttpResponseMessage> PutAsJsonAsync<TRequest>(this HttpClient httpClient, TRequest request, CancellationToken cancellationToken) where TRequest : ISpotifyRequest
    {
        return await httpClient.PutAsJsonAsync(request.Uri(), request, jsonSerializerOptions, cancellationToken);
    }

    public static async Task<TResponse> GetFromJsonAsync<TResponse>(this HttpClient httpClient, ISpotifyRequest request, CancellationToken cancellationToken)
    {
        return await httpClient.GetFromJsonAsync<TResponse>(request.Uri(), jsonSerializerOptions, cancellationToken) ?? throw new SerializationException($"Failed to deserialize response from GET {request.Uri()}");
    }

    public static async Task<TResponse> PostAsFormUrlEncoded<TResponse>(this HttpClient httpClient, ISpotifyRequestFormUrlEncodeable request, CancellationToken cancellationToken)
    {
        var content = new FormUrlEncodedContent(request.FormContent());
        var response = await httpClient.PostAsync(request.Uri(), content, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(jsonSerializerOptions, cancellationToken) ?? throw new SerializationException($"Failed to deserialize response from POST {request.Uri()}");
    }
}