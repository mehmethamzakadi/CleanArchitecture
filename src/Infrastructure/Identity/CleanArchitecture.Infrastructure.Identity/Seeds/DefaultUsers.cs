using CleanArchitecture.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Identity.Seeds;

public static class DefaultUsers
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
    {
        var defaultUser = ApplicationUser.Create("admin", "admin", "admin@admin.com");

        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                await userManager.AddToRoleAsync(defaultUser, "SuperAdmin");
                await userManager.AddToRoleAsync(defaultUser, "Admin");
                await userManager.AddToRoleAsync(defaultUser, "Basic");
            }
        }
    }
} 