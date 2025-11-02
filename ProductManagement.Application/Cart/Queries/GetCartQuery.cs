using MediatR;
using ProductManagement.Application.Cart.DTOs;
using ProductManagement.Domain.Cart.ValueObjects;

namespace ProductManagement.Application.Cart.Queries;

public record GetCartQuery(Guid UserId) : IRequest<CartResponse?>
{
    public UserId GetUserId() => Domain.Cart.ValueObjects.UserId.Create(UserId);
}