using MediatR;
using ProductManagement.Application.Cart.DTOs;
using ProductManagement.Domain.Cart.ValueObjects;
using ProductManagement.Domain.Common.ValueObjects;

namespace ProductManagement.Application.Cart.Commands;

public record AddItemToCartCommand(
    Guid UserId,
    Guid ProductId,
    int Quantity
) : IRequest<CartResponse>
{
    public UserId GetUserId() => Domain.Cart.ValueObjects.UserId.Create(UserId);
    public ProductId GetProductId() => Domain.Common.ValueObjects.ProductId.Create(ProductId);
}