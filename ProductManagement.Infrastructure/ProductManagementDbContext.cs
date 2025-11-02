using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Categories;
using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Orders;
using ProductManagement.Domain.Products;
using ProductManagement.Infrastructure.Configurations;
using ProductManagement.Infrastructure.Persistence.Interceptors;

namespace ProductManagement.Infrastructure;

public class ProductManagementDbContext : DbContext
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;

    public ProductManagementDbContext(DbContextOptions<ProductManagementDbContext> options, PublishDomainEventsInterceptor publishDomainEventsInterceptor)
        : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(ProductManagementDbContext).Assembly);

        base.OnModelCreating(modelBuilder);

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}