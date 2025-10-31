using MediatR;
using ProductManagement.Application.Products.DTOs;

namespace ProductManagement.Application.Products.Commands;

public record AddStockCommand(
    Guid ProductId,
    int Quantity
) : IRequest<ProductResponse>
{
    public Domain.Products.ValueObjects.ProductId GetProductId()
        => Domain.Products.ValueObjects.ProductId.Create(ProductId);
}