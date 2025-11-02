using MediatR;
using ProductManagement.Application.Cart.DTOs;
using ProductManagement.Domain.Cart.ValueObjects;

namespace ProductManagement.Application.Cart.Commands;

public record RemoveItemFromCartCommand(
    Guid UserId,
    Guid CartItemId
) : IRequest<CartResponse>
{
    public UserId GetUserId() => Domain.Cart.ValueObjects.UserId.Create(UserId);
    public CartItemId GetCartItemId() => Domain.Cart.ValueObjects.CartItemId.Create(CartItemId);
}