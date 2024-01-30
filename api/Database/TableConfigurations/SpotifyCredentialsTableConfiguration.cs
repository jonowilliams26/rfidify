using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFIDify.Spotify.Data;

namespace RFIDify.Database.TableConfigurations;

public class SpotifyCredentialsTableConfiguration : IEntityTypeConfiguration<SpotifyCredentials>
{
    public void Configure(EntityTypeBuilder<SpotifyCredentials> builder)
    {
        builder.HasKey(credentials => credentials.ClientId);
    }
}
