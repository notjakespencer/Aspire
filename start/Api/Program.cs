using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

builder.AddRedisOutputCache("cache");

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddNwsManager();

// builder.Configuration.AddAzureKeyVaultSecrets(connectionName: "key-vault");

builder.Services.AddOpenTelemetry()
    .WithMetrics(m => m.AddMeter("NwsManagerMetrics"))
    .WithTracing(m => m.AddSource("NwsManager"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Map the endpoints for the API
app.MapApiEndpoints();

app.Run();
