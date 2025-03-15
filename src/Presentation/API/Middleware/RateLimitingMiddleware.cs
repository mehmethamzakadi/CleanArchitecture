using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;

namespace CleanArchitecture.API.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;
        private readonly int _maxRequests = 100; // Maximum requests per window
        private readonly TimeSpan _window = TimeSpan.FromMinutes(1); // Time window

        public RateLimitingMiddleware(RequestDelegate next, IDistributedCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var cacheKey = $"rate_limit_{ipAddress}";

            var requestInfo = await GetRequestInfoAsync(cacheKey);
            if (requestInfo == null)
            {
                requestInfo = new RequestInfo
                {
                    Count = 1,
                    WindowStart = DateTime.UtcNow
                };
            }
            else if (DateTime.UtcNow > requestInfo.WindowStart.Add(_window))
            {
                requestInfo.Count = 1;
                requestInfo.WindowStart = DateTime.UtcNow;
            }
            else if (requestInfo.Count >= _maxRequests)
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                await context.Response.WriteAsJsonAsync(new { message = "Too many requests. Please try again later." });
                return;
            }
            else
            {
                requestInfo.Count++;
            }

            await UpdateRequestInfoAsync(cacheKey, requestInfo);
            await _next(context);
        }

        private async Task<RequestInfo?> GetRequestInfoAsync(string key)
        {
            var value = await _cache.GetStringAsync(key);
            return value == null ? null : JsonSerializer.Deserialize<RequestInfo>(value);
        }

        private async Task UpdateRequestInfoAsync(string key, RequestInfo info)
        {
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(_window);
            
            await _cache.SetStringAsync(
                key,
                JsonSerializer.Serialize(info),
                options);
        }

        private class RequestInfo
        {
            public int Count { get; set; }
            public DateTime WindowStart { get; set; }
        }
    }
} 