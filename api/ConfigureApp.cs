namespace RFIDify;

public static class ConfigureApp
{
    public static void Configure(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
    }
}
