using RFIDify.Spotify.Apis.WebApi;
using RFIDify.Spotify.Apis.WebApi.RequestResponse.Items;

namespace RFIDify.Spotify.BackgroundServices;

/// <summary>
/// A background service that refreshes the artwork of the playlists every hour.
/// See: https://developer.spotify.com/documentation/web-api/concepts/playlists
/// </summary>
public class RefreshPlaylistsArtworkBackgroundService(ILogger<RefreshPlaylistsArtworkBackgroundService> logger, IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Refreshing playlists artwork...");
            using var scope = serviceScopeFactory.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var api = scope.ServiceProvider.GetRequiredService<ISpotifyWebApi>();
            var rfids = await database.RFIDs.ToListAsync(stoppingToken);
            var rfidsWithPlaylist = rfids.Where(x => x.SpotifyItem.Type is SpotifyItemType.playlist);
            foreach (var rfid in rfidsWithPlaylist)
            {
                rfid.SpotifyItem = await api.GetPlaylist(rfid.SpotifyItem.Id, stoppingToken);
            }

            await database.SaveChangesAsync(stoppingToken);
            logger.LogInformation("Playlists artwork successfully refreshed");
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
