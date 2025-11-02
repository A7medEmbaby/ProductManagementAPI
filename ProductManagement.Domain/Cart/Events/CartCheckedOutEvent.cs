using ProductManagement.Domain.Cart.ValueObjects;
using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Common.ValueObjects;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Domain.Cart.Events;

public record CartCheckedOutEvent(
    CartId CartId,
    UserId UserId,
    List<CartCheckoutItem> Items,
    Money TotalAmount,
    DateTime OccurredAt
) : IDomainEvent
{
    public static CartCheckedOutEvent Create(CartId cartId, UserId userId,
        List<CartCheckoutItem> items, Money totalAmount)
        => new(cartId, userId, items, totalAmount, DateTime.UtcNow);
}

public record CartCheckoutItem(
    ProductId ProductId,
    ProductName ProductName,
    int Quantity,
    Money UnitPrice
);