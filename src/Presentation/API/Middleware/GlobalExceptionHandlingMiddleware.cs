using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";
            
            var problemDetails = new ProblemDetails
            {
                Status = GetStatusCode(exception),
                Title = GetTitle(exception),
                Detail = exception.Message,
                Instance = context.Request.Path,
                Type = $"https://httpstatuses.com/{GetStatusCode(exception)}"
            };

            var json = JsonSerializer.Serialize(problemDetails);
            context.Response.StatusCode = problemDetails.Status.Value;
            await context.Response.WriteAsync(json);
        }

        private static int GetStatusCode(Exception exception)
        {
            return exception switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                InvalidOperationException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }

        private static string GetTitle(Exception exception)
        {
            return exception switch
            {
                ArgumentException => "Bad Request",
                UnauthorizedAccessException => "Unauthorized",
                KeyNotFoundException => "Not Found",
                InvalidOperationException => "Bad Request",
                _ => "Internal Server Error"
            };
        }
    }
} 