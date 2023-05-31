using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using static Discount.Grpc.Protos.DiscountProtoService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//REDIS configuration
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

//GRPC Client configuration
builder.Services.AddGrpcClient<DiscountProtoServiceClient>(option =>
{
    option.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]);
});
builder.Services.AddScoped<DiscountGrpcService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
