using RFIDify.Api;
using RFIDify.RFID.Hubs;
using Serilog;

namespace RFIDify;

public static class ConfigureApp
{
    public static async Task Configure(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseCors();
        app.MapEndpoints();
        app.MapHub<RFIDHub>("/rfidhub");
        await app.EnsureDatabaseCreated();
    }

    private static async Task EnsureDatabaseCreated(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();
        logger.LogInformation("Ensuring database is created...");
        var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await database.Database.MigrateAsync();
        logger.LogInformation("Database successfully created or updated");
    }
}
