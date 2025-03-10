using Library.Data.Models.Security;
using Library.Infrastructure.Repositories;
using Library.Infrastructure.Seed;
using Library.Services.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure
{
    public static class InfrastructureDI
    {
        public static async Task<IServiceCollection> AddInfrastructureDependenciesAsync(
              this IServiceCollection services,
        IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetValue<string>("Local:ConnectionString"));
            });

            services.AddIdentity<UserSet, RoleSet>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddApiEndpoints()
                .AddDefaultTokenProviders();
         

            await SeedData.Initialize(services);

            services.AddScoped<IPatronRepository,PatronRepository>();
            services.AddScoped<IBooksRepository,BooksRepository>();
            services.AddScoped<IBorrowingRepository,BorrowingRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            return services;
        }
    }
}
