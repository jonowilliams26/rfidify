using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

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
        builder.Property(x => x.SpotifyItem).HasJsonConversion();
    }
}

public static class TableConfigurationsExtensions
{
    /// <summary>
    /// Need this to store polymorphic JSON in the database.
    /// This feature was added in .NET 7
    /// See: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/polymorphism?pivots=dotnet-8-0#see-also
    /// 
    /// However, EF Core doesnt support it, so need this rather than using OwnsOne(x => x.ToJson()).
    /// See: https://github.com/dotnet/efcore/issues/27779
    /// </summary>
    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder)
    {
        return propertyBuilder.HasConversion
        (
            value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
            json => JsonSerializer.Deserialize<T>(json, JsonSerializerOptions.Default)!,
            new ValueComparer<T>
            (
                (l, r) => JsonSerializer.Serialize(l, JsonSerializerOptions.Default) == JsonSerializer.Serialize(r, JsonSerializerOptions.Default),
                v => v == null ? 0 : JsonSerializer.Serialize(v, JsonSerializerOptions.Default).GetHashCode(),
                v => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v, JsonSerializerOptions.Default), JsonSerializerOptions.Default)!
            )
        );
    }
}