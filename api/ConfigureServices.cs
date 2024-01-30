using RFIDify.Services;
using RFIDify.Spotify.Apis.DelegatingHandlers;
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
        builder.Services.AddValidatorsFromAssembly(typeof(ConfigureServices).Assembly, includeInternalTypes: true);
        builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
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
        // Add the delegate handlers
        builder.Services.AddTransient<EnsureSuccessDelgatingHandler>();
        builder.Services.AddTransient<LoggingDelegatingHandler>();
        builder.Services.AddTransient<ClientIdAndSecretAuthenticationDelegatingHandler>();

        builder.Services.Configure<SpotifyAccountsApiOptions>(builder.Configuration.GetSection("Spotify:AccountsApi"));
        builder.Services.AddHttpClient<ISpotifyAccountsApi, SpotifyAccountsApi>(client =>
        {
            var baseUrl = builder.Configuration["Spotify:AccountsApi:BaseUrl"];
            client.BaseAddress = new Uri(baseUrl!);
        })
        .AddHttpMessageHandler<EnsureSuccessDelgatingHandler>()
        .AddHttpMessageHandler<LoggingDelegatingHandler>()
        .AddHttpMessageHandler<ClientIdAndSecretAuthenticationDelegatingHandler>();
    }
}
