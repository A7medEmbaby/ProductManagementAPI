using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.Cart;
using ProductManagement.Application.Categories;
using ProductManagement.Application.Messaging;
using ProductManagement.Application.Orders;
using ProductManagement.Application.Products;
using ProductManagement.Infrastructure.Messaging;
using ProductManagement.Infrastructure.Messaging.Consumers;
using ProductManagement.Infrastructure.Persistence.Interceptors;
using ProductManagement.Infrastructure.Repositories;

namespace ProductManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<PublishDomainEventsInterceptor>();

        // Register DbContext with SQLite and the interceptor
        services.AddDbContext<ProductManagementDbContext>((serviceProvider, options) =>
        {
            // Get the interceptor from DI
            var interceptor = serviceProvider.GetRequiredService<PublishDomainEventsInterceptor>();

            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=../Database/ProductManagement.db")
                .AddInterceptors(interceptor);
        });

        // Register Repositories (binding interfaces to implementations)
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddSingleton<ICartRepository, InMemoryCartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();


        // Register Cart Repository as Singleton (in-memory)
        services.AddSingleton<ICartRepository, InMemoryCartRepository>();

        // RabbitMQ Connection Factory
        services.AddSingleton<RabbitMQConnectionFactory>();

        // Message Bus
        services.AddSingleton<IMessageBus, RabbitMQMessageBus>();

        // Consumers (Scoped for use in background services)
        services.AddScoped<OrderCreationConsumer>();
        services.AddScoped<StockDeductionConsumer>();
        services.AddScoped<CartClearingConsumer>();

        return services;
    }
}