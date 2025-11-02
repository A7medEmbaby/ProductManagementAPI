using ProductManagement.Domain.Cart.ValueObjects;
using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Common.ValueObjects;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Domain.Cart.Events;

public record CartItemAddedEvent(
    CartId CartId,
    UserId UserId,
    ProductId ProductId,
    ProductName ProductName,
    int Quantity,
    Money UnitPrice,
    DateTime OccurredAt
) : IDomainEvent
{
    public static CartItemAddedEvent Create(CartId cartId, UserId userId, ProductId productId,
        ProductName productName, int quantity, Money unitPrice)
        => new(cartId, userId, productId, productName, quantity, unitPrice, DateTime.UtcNow);
}