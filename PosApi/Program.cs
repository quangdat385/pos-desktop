using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Polly;
using Polly.Extensions.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using PosApi.Data;
using PosApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Logging configuration
builder.Services.AddLoggingConfiguration(builder.Logging);
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// Database configuration
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// API validation configuration
builder.Services.AddApiValidationConfiguration();

// CORS configuration
builder.Services.AddCorsConfiguration();

// JSON configuration
builder.Services.AddJsonConfiguration();

// Swagger/OpenAPI configuration
builder.Services.AddSwaggerConfiguration();

// HttpClient with Polly configuration
builder.Services.AddHttpClientConfiguration();

// Memory Cache configuration
builder.Services.AddMemoryCacheConfiguration();

// Dependency Injection configuration
builder.Services.AddDependencyInjectionConfiguration();

// Rate limiting configuration
builder.Services.AddRateLimitingPolicies();

var app = builder.Build();

// Configure pipeline
app.ConfigurePipeline();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        if (dbContext.Database.CanConnect())
        {
            Log.Information("Database connection successful.");
            // ✅ Seed Production Data
            await dbContext.Database.MigrateAsync();
            await PosApi.Seeders.ProductSeeders.SeedProductsAsync(dbContext);
        }
        else
        {
            Log.Error("Database connection failed.");
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Database connection error.");
    }
}

app.Run();
