using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Domain.Categories;
using ProductManagement.Domain.Products;
using ProductManagement.Infrastructure.Data;
using ProductManagement.Infrastructure.Data.Repositories;
using ProductManagement.Infrastructure.Events;

namespace ProductManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register DbContext with SQLite
        services.AddDbContext<ProductManagementDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=../Database/ProductManagement.db"));

        // Register Repositories (binding interfaces to implementations)
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        // Register Domain Event Dispatcher
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}