using Serilog;

namespace RFIDify;

public static class ConfigureServices
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.AddSerilog();
        builder.AddSwagger();
        builder.AddDatabase();
        builder.AddSpotifyAccountsApi();
    }

    private static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    private static void AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        });
    }

    private static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite("Data Source=database/app.db");
        });
    }

    private static void AddSpotifyAccountsApi(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<SpotifyAccountsApiOptions>(builder.Configuration.GetSection("Spotify:AccountsApi"));
        builder.Services.AddHttpClient<ISpotifyAccountsApi, SpotifyAccountsApi>(client =>
        {
            var baseUrl = builder.Configuration["Spotify:AccountsApi:BaseUrl"];
            client.BaseAddress = new Uri(baseUrl!);
        });
    }
}
