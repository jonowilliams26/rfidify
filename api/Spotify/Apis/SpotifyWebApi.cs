﻿using RFIDify.Spotify.Apis.Extensions;
using System.Text.Json.Serialization;

namespace RFIDify.Spotify.Apis;

public interface ISpotifyWebApi
{
    Task<Track> GetTrack(string id, CancellationToken cancellationToken);
    Task Play(SpotifyId id, CancellationToken cancellationToken);
}

public class SpotifyWebApi(HttpClient httpClient) : ISpotifyWebApi
{
    public async Task Play(SpotifyId id, CancellationToken cancellationToken)
    {
        PlayRequest request = id.Type switch
        {
            SpotifyIdType.Track => new() { Uris = [id.Id] },
            _ => new() { ContextUri = id.Id }
        };

        await httpClient.PutAsJsonAsync("me/player/play", request, cancellationToken);
    }

    public Task<Track> GetTrack(string id, CancellationToken cancellationToken) => httpClient.Get<Track>($"tracks/{id}", cancellationToken);

    private record PlayRequest
    {
        [JsonPropertyName("context_uri")]
        public string? ContextUri { get; init; }

        [JsonPropertyName("uris")]
        public string[]? Uris { get; init; }
    }
}
