using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Domain.Products;
using ProductManagement.Domain.Products.ValueObjects;
using ProductManagement.Domain.Categories.ValueObjects;

namespace ProductManagement.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,                  // To DB: Extract Guid from AggregateRootId
                value => ProductId.Create(value)); // From DB: Create ProductId from Guid

        // Product name value object
        builder.Property(p => p.Name)
            .HasConversion(name => name.Value, value => new ProductName(value))
            .HasMaxLength(200)
            .IsRequired();

        // CategoryId - Still needs conversion (it's a Value Object reference)
        builder.Property(p => p.CategoryId)
            .HasConversion(id => id.Value, value => CategoryId.Create(value))
            .IsRequired();

        // Money value object (owned entity)
        builder.OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Amount)
                .HasColumnName("PriceAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            price.Property(p => p.Currency)
                .HasColumnName("PriceCurrency")
                .HasMaxLength(3)
                .IsRequired();
        });

        // Timestamps
        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt);

        // Domain events (ignored - handled separately)
        builder.Ignore(p => p.DomainEvents);

        // Indexes
        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.CreatedAt);
    }
}