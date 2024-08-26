using CryptoTest.Persistence;
using CryptoTest.Persistence.Interfaces;
using CryptoTest.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var services = builder.Services;
var configuration = builder.Configuration;
var loggerFactory = builder.Logging;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

loggerFactory.ClearProviders();
loggerFactory.AddConsole();
loggerFactory.AddDebug();

services.AddSingleton(new SqlDatabase(connectionString));

services.AddScoped<IMessageRepository, MessageRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
