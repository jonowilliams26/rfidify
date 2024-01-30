using RFIDify.Spotify.Data;

namespace RFIDify.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SpotifyCredentials> SpotifyCredentials { get; set; }
    public DbSet<SpotifyAccessToken> SpotifyAccessToken { get; set; }
    public DbSet<SpotifyRefreshToken> SpotifyRefreshToken { get; set; }
    public DbSet<SpotifyAuthorizationState> SpotifyAuthorizationState { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}