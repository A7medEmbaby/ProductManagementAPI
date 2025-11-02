using ProductManagement.Domain.Cart.ValueObjects;
using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Common.ValueObjects;

namespace ProductManagement.Domain.Cart.Events;

public record CartItemRemovedEvent(
    CartId CartId,
    UserId UserId,
    CartItemId CartItemId,
    ProductId ProductId,
    int Quantity,
    DateTime OccurredAt
) : IDomainEvent
{
    public static CartItemRemovedEvent Create(
        CartId cartId,
        UserId userId,
        CartItemId cartItemId,
        ProductId productId,
        int quantity)
        => new(cartId, userId, cartItemId, productId, quantity, DateTime.UtcNow);
}