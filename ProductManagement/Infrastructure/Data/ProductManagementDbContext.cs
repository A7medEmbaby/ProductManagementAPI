using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Products;
using ProductManagement.Domain.Categories;
using ProductManagement.Infrastructure.Data.Configurations;

namespace ProductManagement.Infrastructure.Data;

public class ProductManagementDbContext : DbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    public ProductManagementDbContext(DbContextOptions<ProductManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }
}