using EventBus.Messages.Common;
using EventBus.Messages.Events;
using MassTransit;
using Order.API.EventBusConsumer;
using Order.API.Extensions;
using Order.Application;
using Order.Infrastructure;
using Order.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);




//AUTOMAPPER
builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddScoped<BasketCheckoutConsumer>();

//RABBITMQ-MASSTRANSIT Configuration
builder.Services.AddMassTransit((config) =>
{
    //make order.api a consumer of basket.api publisher
    config.AddConsumer<BasketCheckoutConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

        //add a receiver endpoint
        cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });
    });
});

var app = builder.Build();

// DB Migration
//app.Services.MigrateDatabase<OrderContext>((context, services) =>
//{
//    var logger = services.GetRequiredService<ILogger<OrderContextSeed>>();
//    OrderContextSeed.SeedAsync(context, logger).Wait();
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
