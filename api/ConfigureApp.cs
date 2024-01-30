using Serilog;

namespace RFIDify;

public static class ConfigureApp
{
    public static void Configure(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
    }
}
