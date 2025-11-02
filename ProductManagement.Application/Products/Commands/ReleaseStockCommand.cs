using MediatR;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.Common.ValueObjects;

namespace ProductManagement.Application.Products.Commands;

public record ReleaseStockCommand(
    Guid ProductId,
    int Quantity
) : IRequest<ProductResponse>
{
    public ProductId GetProductId()
        => Domain.Common.ValueObjects.ProductId.Create(ProductId);
}