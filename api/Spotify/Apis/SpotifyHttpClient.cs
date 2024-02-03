using System.Runtime.Serialization;
using System.Text.Json;

namespace RFIDify.Spotify.Apis;

/// <summary>
/// A wrapper around <see cref="HttpClient"/> that provides methods for sending requests to the Spotify Web API using the proper JSON serialization settings.
/// </summary>
/// <param name="httpClient"></param>
public class SpotifyHttpClient(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public Uri? BaseAddress => httpClient.BaseAddress;

    /// <summary>
    /// Sends a GET request to the specified uri and deserializes the JSON response to the specified type.
    /// </summary>
    /// <exception cref="SerializationException"></exception>
    public async Task<TResponse> Get<TResponse>(string uri, CancellationToken cancellationToken)
    {
        return await httpClient.GetFromJsonAsync<TResponse>(uri, jsonSerializerOptions, cancellationToken) ?? throw new SerializationException($"Failed to deserialize response from GET {uri}");
    }

    /// <summary>
    /// Sends as PUT request to the specified uri containing the value serialized as JSON.
    /// </summary>
    public async Task<HttpResponseMessage> Put<T>(string uri, T value, CancellationToken cancellationToken)
    {
        return await httpClient.PutAsJsonAsync(uri, value, jsonSerializerOptions, cancellationToken);
    }

    /// <summary>
    /// Sends a POST request to the specified uri containing the <paramref name="values"/> serialized as <c>application/x-www-form-urlencoded</c> and deserializes the JSON response to the specified type.
    /// </summary>
    /// <exception cref="SerializationException"></exception>
    public async Task<TResponse> Post<TResponse>(string uri, Dictionary<string, string> values, CancellationToken cancellationToken)
    {
        var content = new FormUrlEncodedContent(values);
        var response = await httpClient.PostAsync(uri, content, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(jsonSerializerOptions, cancellationToken) ?? throw new SerializationException($"Failed to deserialize response from POST {uri}");
    }
}