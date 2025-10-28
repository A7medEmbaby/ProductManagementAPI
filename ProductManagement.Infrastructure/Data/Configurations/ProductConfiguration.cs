using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Domain.Products;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        // Primary key
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, value => new ProductId(value))
            .ValueGeneratedNever();

        // Product name value object
        builder.Property(p => p.Name)
            .HasConversion(name => name.Value, value => new ProductName(value))
            .HasMaxLength(200)
            .IsRequired();

        // CategoryId - ONLY store the ID, no navigation property!
        builder.Property(p => p.CategoryId)
            .HasConversion(id => id.Value, value => new CategoryId(value))
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