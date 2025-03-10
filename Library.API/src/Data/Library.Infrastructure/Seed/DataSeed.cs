using Library.Data.Models.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.Seed
{
    public static class SeedData
    {
        public async static Task EnsureDatabaseExistAsync(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
        }


        public static async Task Initialize(IServiceCollection services)
        {

            var provider = services.BuildServiceProvider();
            var context = provider.GetService<ApplicationDbContext>();

            // Ensure the database is created
            await EnsureDatabaseExistAsync(context);

            if (!context.Set<UserSet>().Any())
            {
                await SeedUsers(services, context);
            }

        }

        private static async Task SeedUsers(IServiceCollection services, ApplicationDbContext context)
        {
            var provider = services.BuildServiceProvider();

            using var scope = provider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserSet>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleSet>>();

            // Seed Roles
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new RoleSet { Name = role });
                }
            }

            // Seed Admin User
            var adminEmail = "admin@example.com";
            var adminPassword = "Admin@123";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new UserSet
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    if (await roleManager.RoleExistsAsync("Admin"))
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }
            }

            await context.SaveChangesAsync();

        }
    }
}