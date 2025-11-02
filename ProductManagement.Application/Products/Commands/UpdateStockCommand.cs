using MediatR;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.Common.ValueObjects;

namespace ProductManagement.Application.Products.Commands;

public record UpdateStockCommand(
    Guid ProductId,
    int NewQuantity
) : IRequest<ProductResponse>
{
    public ProductId GetProductId()
        => Domain.Common.ValueObjects.ProductId.Create(ProductId);
}