using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Identity.Context;
using CleanArchitecture.Infrastructure.Identity.Models;
using CleanArchitecture.Infrastructure.Identity.Seeds;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Program).Assembly)
    .AddControllersAsServices();

builder.Services.AddEndpointsApiExplorer();

// Add Swagger services
builder.Services.AddSwaggerDocumentation();

// Add HealthCheck services
builder.Services.AddHealthChecks(builder.Configuration);

// Add Application services
builder.Services.AddApplication();

// Identity servislerini ekle
builder.Services.AddIdentityInfrastructure(builder.Configuration);

// Add Authorization services
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseHttpsRedirection();

// Kimlik doğrulama ve yetkilendirme middleware'lerini ekle
app.UseAuthentication();
app.UseAuthorization();

// Add HealthCheck middleware
app.UseHealthChecks();

app.MapControllers();

// Create database and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<IdentityContext>();
    context.Database.EnsureCreated();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    await DefaultRoles.SeedAsync(roleManager);
    await DefaultUsers.SeedAsync(userManager);
}

app.Run();
