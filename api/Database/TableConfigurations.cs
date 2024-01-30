using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RFIDify.Spotify.Data;

namespace RFIDify.Database;

public class TableConfigurations : 
    IEntityTypeConfiguration<SpotifyCredentials>, 
    IEntityTypeConfiguration<SpotifyAccessToken>, 
    IEntityTypeConfiguration<SpotifyRefreshToken>, 
    IEntityTypeConfiguration<SpotifyAuthorizationState>,
    IEntityTypeConfiguration<RFIDTag>
{
    public void Configure(EntityTypeBuilder<SpotifyCredentials> builder)
    {
        builder.HasKey(x => x.ClientId);
    }

    public void Configure(EntityTypeBuilder<SpotifyAccessToken> builder)
    {
        builder.HasKey(x => x.Token);
    }

    public void Configure(EntityTypeBuilder<SpotifyRefreshToken> builder)
    {
        builder.HasKey(x => x.Token);
    }

    public void Configure(EntityTypeBuilder<SpotifyAuthorizationState> builder)
    {
        builder.HasKey(x => x.State);
    }

    public void Configure(EntityTypeBuilder<RFIDTag> builder)
    {
        builder.ComplexProperty(x => x.SpotifyId);
    }
}
