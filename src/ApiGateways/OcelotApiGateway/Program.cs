using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure environment specific json file
builder.Host.ConfigureAppConfiguration((hostContext, config) => {
    config.AddJsonFile($"ocelot.{hostContext.HostingEnvironment.EnvironmentName}.json",true,true);
});

// Configure logging to view the logs from OCELOT APIGATEWAY service
builder.Host.ConfigureLogging(logging => {
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

// Add OCELOT APIGATEWAY service and cache manager
builder.Services.AddOcelot()
                .AddCacheManager(settings => settings.WithDictionaryHandle());


var app = builder.Build();
app.MapGet("/", () => "Hello World!");

// Use OCELOT APIGATEWAY service
await app.UseOcelot();

app.Run();
