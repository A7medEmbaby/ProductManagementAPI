using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Domain.Categories;
using ProductManagement.Domain.Categories.ValueObjects;

namespace ProductManagement.Infrastructure.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.AggregateId);
        builder.Ignore(c => c.Id); // Ignore inherited Id property from Entity<TId>

        builder.Property(c => c.AggregateId)
            .HasColumnName("Id")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,                    // To DB: Extract Guid from AggregateRootId
                value => CategoryId.Create(value)); // From DB: Create CategoryId from Guid

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