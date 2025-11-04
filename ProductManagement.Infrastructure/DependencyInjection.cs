using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.Cart;
using ProductManagement.Application.Categories;
using ProductManagement.Application.Orders;
using ProductManagement.Application.Products;
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

        // MassTransit Configuration
        services.AddMassTransit(x =>
        {
            // Register consumers
            x.AddConsumer<OrderCreationConsumer>();
            x.AddConsumer<StockDeductionConsumer>();
            x.AddConsumer<CartClearingConsumer>();

            // Configure RabbitMQ transport
            x.UsingRabbitMq((context, cfg) =>
            {
                var host = configuration["RabbitMQ:Host"] ?? "rabbitmq://localhost";
                var username = configuration["RabbitMQ:Username"] ?? "guest";
                var password = configuration["RabbitMQ:Password"] ?? "guest";

                cfg.Host(host, h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                // Configure retry policy (3 attempts with exponential backoff)
                cfg.UseMessageRetry(r => r.Exponential(
                    retryLimit: 3,
                    minInterval: TimeSpan.FromSeconds(1),
                    maxInterval: TimeSpan.FromSeconds(4),
                    intervalDelta: TimeSpan.FromSeconds(1)
                ));

                // Configure endpoints for consumers
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}