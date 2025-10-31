using MediatR;
using ProductManagement.Application.Products.DTOs;

namespace ProductManagement.Application.Products.Commands;

public record UpdateStockCommand(
    Guid ProductId,
    int NewQuantity
) : IRequest<ProductResponse>
{
    public Domain.Products.ValueObjects.ProductId GetProductId()
        => Domain.Products.ValueObjects.ProductId.Create(ProductId);
}