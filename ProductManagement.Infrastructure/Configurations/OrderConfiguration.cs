using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Domain.Cart.ValueObjects;
using ProductManagement.Domain.Common.ValueObjects;
using ProductManagement.Domain.Orders;
using ProductManagement.Domain.Orders.ValueObjects;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.AggregateId);
        builder.Ignore(o => o.Id); // Ignore inherited Id property from Entity<TId>

        builder.Property(o => o.AggregateId)
            .HasColumnName("Id")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value));

        // UserId value object
        builder.Property(o => o.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value))
            .IsRequired();

        // OrderItems list
        builder.OwnsMany(o => o.Items, itemBuilder =>
        {
            itemBuilder.ToTable("OrderItems");

            itemBuilder.WithOwner().HasForeignKey("OrderId");

            itemBuilder.HasKey("Id", "OrderId");

            itemBuilder.Property(i => i.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => OrderItemId.Create(value));

            itemBuilder.Property(i => i.ProductId)
                .HasConversion(
                    id => id.Value,
                    value => ProductId.Create(value));

            itemBuilder.Property(i => i.ProductName)
                .HasConversion(
                    name => name.Value,
                    value => new ProductName(value));

            itemBuilder.OwnsOne(i => i.UnitPrice, priceBuilder =>
            {
                priceBuilder.Property(p => p.Amount)
                    .HasColumnName("UnitPriceAmount")
                    .HasColumnType("decimal(18,2)");

                priceBuilder.Property(p => p.Currency)
                    .HasColumnName("UnitPriceCurrency");
            });

            itemBuilder.Ignore(i => i.LineTotal);
        });

        builder.Metadata.FindNavigation(nameof(Order.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        // Order Status as enum
        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        // Timestamps
        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.UpdatedAt);
        builder.Property(o => o.CompletedAt);
        builder.Property(o => o.CancelledAt);

        // Domain events (ignored)
        builder.Ignore(o => o.DomainEvents);

        // TotalAmount (computed from items, not stored)
        builder.Ignore(o => o.TotalAmount);

        // Indexes
        builder.HasIndex(o => o.UserId);
        builder.HasIndex(o => o.Status);
        builder.HasIndex(o => o.CreatedAt);
    }
}