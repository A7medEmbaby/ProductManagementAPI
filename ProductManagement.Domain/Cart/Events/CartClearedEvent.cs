using ProductManagement.Domain.Cart.ValueObjects;
using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Common.ValueObjects;

namespace ProductManagement.Domain.Cart.Events;

public record CartClearedEvent(
    CartId CartId,
    UserId UserId,
    List<ClearedCartItem> Items,
    DateTime OccurredAt
) : IDomainEvent
{
    public static CartClearedEvent Create(CartId cartId, UserId userId, List<ClearedCartItem> items)
        => new(cartId, userId, items, DateTime.UtcNow);
}

public record ClearedCartItem(
    ProductId ProductId,
    int Quantity
);