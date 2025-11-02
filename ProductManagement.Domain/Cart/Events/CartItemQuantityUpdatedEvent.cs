using ProductManagement.Domain.Cart.ValueObjects;
using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Common.ValueObjects;

namespace ProductManagement.Domain.Cart.Events;

public record CartItemQuantityUpdatedEvent(
    CartId CartId,
    UserId UserId,
    CartItemId CartItemId,
    ProductId ProductId,
    int OldQuantity,
    int NewQuantity,
    DateTime OccurredAt
) : IDomainEvent
{
    public static CartItemQuantityUpdatedEvent Create(
        CartId cartId,
        UserId userId,
        CartItemId cartItemId,
        ProductId productId,
        int oldQuantity,
        int newQuantity)
        => new(cartId, userId, cartItemId, productId, oldQuantity, newQuantity, DateTime.UtcNow);
}