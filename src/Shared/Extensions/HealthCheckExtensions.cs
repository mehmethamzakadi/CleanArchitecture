using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text;
using HealthChecks.NpgSql;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Shared.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddNpgSql(
                configuration.GetConnectionString("DefaultConnection") ?? "",
                name: "Database",
                tags: new[] { "db", "postgresql" })
            .AddRedis(
                configuration.GetConnectionString("Redis") ?? "",
                name: "Redis",
                tags: new[] { "cache", "redis" })
            .AddUrlGroup(
                new Uri(configuration["ExternalServices:PaymentApi"] ?? ""),
                name: "Payment API",
                tags: new[] { "external", "payment" })
            .AddUrlGroup(
                new Uri(configuration["ExternalServices:NotificationApi"] ?? ""),
                name: "Notification API",
                tags: new[] { "external", "notification" });

        return services;
    }

    public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    Status = report.Status.ToString(),
                    Duration = report.TotalDuration,
                    Components = report.Entries.Select(e => new
                    {
                        Component = e.Key,
                        Status = e.Value.Status.ToString(),
                        Description = e.Value.Description,
                        Duration = e.Value.Duration,
                        Tags = e.Value.Tags
                    })
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
            }
        });

        return app;
    }
} 