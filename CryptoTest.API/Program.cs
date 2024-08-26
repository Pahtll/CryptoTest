using CryptoTest.API.Endpoints;
using CryptoTest.API.Hubs;
using CryptoTest.Application.Interfaces;
using CryptoTest.Application.Services;
using CryptoTest.Persistence;
using CryptoTest.Persistence.Interfaces;
using CryptoTest.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var defaultConnection = configuration.GetConnectionString("DefaultConnection");
var redisConnection = configuration.GetConnectionString("RedisConnection");
var loggerFactory = builder.Logging;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSignalR();
services.AddControllers();

services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
    options.InstanceName = "CryptoTest";
});

services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // TODO: Change this to a specific origin
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

loggerFactory.ClearProviders();
loggerFactory.AddConsole();
loggerFactory.AddDebug();

services.AddSingleton(new SqlDatabase(configuration));

services.AddScoped<IMessageRepository, MessageRepository>();
services.AddScoped<IMessageService, MessageService>();

var app = builder.Build();

app.MapControllers();
app.UseCors();

app.UseHttpsRedirection();

app.MapHub<ChatHub>("/chat");
app.MapMessageEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
