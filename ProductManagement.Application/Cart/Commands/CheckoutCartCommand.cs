using MediatR;
using ProductManagement.Application.Cart.DTOs;
using ProductManagement.Domain.Cart.ValueObjects;

namespace ProductManagement.Application.Cart.Commands;

public record CheckoutCartCommand(
    Guid UserId
) : IRequest<CartResponse>
{
    public UserId GetUserId() => Domain.Cart.ValueObjects.UserId.Create(UserId);
}