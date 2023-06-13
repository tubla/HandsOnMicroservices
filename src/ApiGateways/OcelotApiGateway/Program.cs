using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Configure logging to view the logs from OCELOT APIGATEWAY service
builder.Host.ConfigureLogging(logging => {
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

// Add OCELOT APIGATEWAY service
builder.Services.AddOcelot();

app.MapGet("/", () => "Hello World!");

// Use OCELOT APIGATEWAY service
await app.UseOcelot();

app.Run();
