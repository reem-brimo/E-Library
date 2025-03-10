using Bogus;
using Library.Data.Models;
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

            //if (!context.Set<Borrow>().Any())
            //{
            //    var products = new List<Borrow>();
            //    for (int i = 1; i <= 10; i++)
            //    {
            //        products.Add(new Borrow
            //        {
            //            Name = $"Product {i}",
            //            Price = 10.0 * i,
            //            Description = $"Description for Product {i}",
            //            Stock = 100 + i,
            //            ImageUrl = $"https://example.com/product{i}.jpg"
            //        });
            //    }
            //    context.AddRange(products);
            //    await context.SaveChangesAsync();
            //}

            //if (!context.Set<Book>().Any())
            //{
            //    var rand = new Random();

            //    var users = context.Users.ToList();
            //    var orders = new List<Book>();
            //    for (int i = 1; i <= 10; i++)
            //    {
            //        int index = rand.Next(users.Count);

            //        orders.Add(new Book
            //        {
            //            UserId = users[index].Id,
            //            OrderDate = DateTime.Now.AddDays(-i),
            //            TotalAmount = 0, // Calculated later based on OrderItems
            //            Address = new Faker().Address.FullAddress()

            //        });
            //    }
            //    context.AddRange(orders);
            //    await context.SaveChangesAsync();
            //}

            //if (!context.Set<Patron>().Any())
            //{
            //    var orders = context.Orders.ToList();
            //    var products = context.Products.ToList();
            //    var random = new Random();
            //    var orderItems = new List<Patron>();
            //    foreach (var order in orders)
            //    {
            //        for (int i = 0; i < 3; i++) // Each order will have 3 items
            //        {
            //            var product = products[random.Next(products.Count)];
            //            var quantity = random.Next(1, 5);
            //            var orderItem = new Patron
            //            {
            //                OrderId = order.Id,
            //                ProductId = product.Id,
            //                Quantity = quantity,
            //            };
            //            orderItems.Add(orderItem);
            //            order.TotalAmount += orderItem.Quantity * product.Price;
            //        }
            //    }
            //    context.AddRange(orderItems);
            //    context.UpdateRange(orders);

            //    await context.SaveChangesAsync();
            //}

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