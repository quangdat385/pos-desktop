using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using PosApi.Data;
using Microsoft.AspNetCore.Http;
using PosApi.Infrastructure.Cache;
using PosApi.Interfaces;
using PosApi.Services;
using PosApi.Repositories;
using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;

namespace PosApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseConfiguration(
        this IServiceCollection services,
     IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=localhost;Port=3306;Database=test_vn;User=root;Password=123456Example@;SslMode=None;";

        services.AddDbContext<AppDbContext>(options =>
  {
      options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
  });

        return services;
    }

    public static IServiceCollection AddApiValidationConfiguration(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
   {
       options.InvalidModelStateResponseFactory = context =>
               {
                   var errors = context.ModelState
              .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                    .ToDictionary(
                   kvp => kvp.Key,
                     kvp => kvp.Value != null
                    ? kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        : Array.Empty<string>()
                   );

                   return new BadRequestObjectResult(new
                   {
                       message = "Validation failed",
                       errors
                   });
               };
   });

        return services;
    }

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: "DevCorsPolicy", policy =>
            {
                policy
                .WithOrigins("http://localhost:5220", "https://localhost:7224", "http://localhost:4000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        });

        return services;
    }

    public static IServiceCollection AddJsonConfiguration(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Ignore null values
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                    System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

                // Ignore circular references (FIX for object cycle error)
                options.JsonSerializerOptions.ReferenceHandler =
                System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

                // Optional: Better formatting
                options.JsonSerializerOptions.WriteIndented = false;

            });

        return services;
    }

    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
      {
          c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Shopdev Api", Version = "v1" });

          // Thêm cấu hình JWT Bearer
          c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
              {
                  Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                  Name = "Authorization",
                  In = ParameterLocation.Header,
                  Type = SecuritySchemeType.Http,
                  Scheme = "bearer",
                  BearerFormat = "JWT"
          });

          c.AddSecurityRequirement(new OpenApiSecurityRequirement
          {
          {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
               {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
          },
             new List<string>()
}
         });
      });

        return services;
    }

    public static IServiceCollection AddHttpClientConfiguration(this IServiceCollection services)
    {
        services.AddHttpClient("Downstream")
           .AddPolicyHandler(HttpPolicyExtensions
           .HandleTransientHttpError()
           .OrResult(r => (int)r.StatusCode == 429)
           .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30)))
                .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(r => (int)r.StatusCode == 429)
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))
           )
        );

        return services;
    }

    public static IServiceCollection AddLoggingConfiguration(
           this IServiceCollection services,
           ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddSerilog(dispose: true);

        return services;
    }

    // Add memory cache and common HTTP context accessor
    public static IServiceCollection AddMemoryCacheConfiguration(this IServiceCollection services)
    {
        services.AddMemoryCache();
        // Register cache service implementation
        services.AddSingleton<ICacheService, MemoryCacheService>();
        return services;
    }

    // Add application dependency injections (services, repositories, etc.)
    public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services)
    {
        // TODO: Register your application services and repositories here
        // services
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();

        // repositories
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
    public static IServiceCollection AddRateLimitingPolicies(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            // Per-IP fixed window limiter (global)
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 20,             // 20 req per second per IP (adjust as needed)
                        Window = TimeSpan.FromSeconds(1),
                        QueueLimit = 0,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }
                )
            );

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // On rejected: add Retry-After header and short message
            options.OnRejected = async (context, token) =>
            {
                var response = context.HttpContext.Response;
                response.StatusCode = StatusCodes.Status429TooManyRequests;

                // Try to get limiter-suggested retry delay
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    response.Headers.RetryAfter = ((int)Math.Ceiling(retryAfter.TotalSeconds))
                        .ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    // Fallback: 1 second
                    response.Headers.RetryAfter = "1";
                }

                if (!response.HasStarted)
                {
                    await response.WriteAsync("Too many requests. Please retry later.", token);
                }
            };

            // Discount endpoints policy (burst control per IP)
            options.AddPolicy("global", ctx =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 10,              // 10 requests /30s per IP
                        Window = TimeSpan.FromSeconds(30),
                        QueueLimit = 0,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }
                )
            );
        });

        return services;
    }

    // Activate middleware
    public static IApplicationBuilder UseRateLimitingPolicies(this IApplicationBuilder app)
    {
        app.UseRateLimiter();
        return app;
    }
}
