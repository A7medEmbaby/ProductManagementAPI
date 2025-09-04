using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Domain.Categories;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        // Primary key
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, value => new CategoryId(value))
            .ValueGeneratedNever();

        // Category name value object
        builder.Property(c => c.Name)
            .HasConversion(name => name.Value, value => new CategoryName(value))
            .HasMaxLength(100)
            .IsRequired();

        // ProductCount - NEW FIELD
        builder.Property(c => c.ProductCount)
            .IsRequired()
            .HasDefaultValue(0);

        // Timestamps
        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt);

        // Domain events (ignored - handled separately)
        builder.Ignore(c => c.DomainEvents);

        // Unique constraint on name
        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.HasIndex(c => c.CreatedAt);

        // Index on ProductCount for potential queries
        builder.HasIndex(c => c.ProductCount);
    }
}