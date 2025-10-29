using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Domain.Categories;
using ProductManagement.Domain.Categories.ValueObjects;

namespace ProductManagement.Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        // ⚠️ KEY CHANGE: Now configures the PRIMITIVE Guid, not CategoryId
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedNever(); // No conversion needed - it's already a Guid!

        // Category name value object
        builder.Property(c => c.Name)
            .HasConversion(name => name.Value, value => new CategoryName(value))
            .HasMaxLength(100)
            .IsRequired();

        // ProductCount
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
        builder.HasIndex(c => c.ProductCount);
    }
}