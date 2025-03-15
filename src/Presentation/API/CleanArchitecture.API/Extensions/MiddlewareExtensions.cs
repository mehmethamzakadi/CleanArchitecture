using Microsoft.AspNetCore.Builder;
using CleanArchitecture.API.Middleware;

namespace CleanArchitecture.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }

        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SecurityHeadersMiddleware>();
        }

        public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RateLimitingMiddleware>();
        }

        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
        {
            return app
                .UseGlobalExceptionHandler()
                .UseSecurityHeaders()
                .UseRateLimiting();
        }
    }
} 