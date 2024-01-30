using System.Runtime.Serialization;

namespace RFIDify.Spotify.Apis.Extensions;

public static class HttpClientExtensions
{
    public static async Task<T> SendAndDeserializeJson<T>(this HttpClient httpClient, HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await httpClient.SendAsync(request, cancellationToken);
        var data = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        if (data is null)
        {
            throw new SerializationException($"Failed to deserialize response from {request.Method} {request.RequestUri}");
        }

        return data;
    }
}
