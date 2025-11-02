using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Common.ValueObjects;
using ProductManagement.Domain.Orders.ValueObjects;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Domain.Orders;

public class OrderItem : Entity<OrderItemId>
{
    public ProductId ProductId { get; private set; }
    public ProductName ProductName { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public Money LineTotal => Money.Create(UnitPrice.Amount * Quantity, UnitPrice.Currency);

    private OrderItem(OrderItemId id, ProductId productId, ProductName productName, int quantity, Money unitPrice)
        : base(id)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    private OrderItem() : base() { } // For EF Core

    public static OrderItem Create(ProductId productId, ProductName productName, int quantity, Money unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        var itemId = OrderItemId.CreateUnique();
        return new OrderItem(itemId, productId, productName, quantity, unitPrice);
    }
}