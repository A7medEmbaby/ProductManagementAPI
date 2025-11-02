using MediatR;
using ProductManagement.Domain.Common.ValueObjects;

namespace ProductManagement.Application.Products.Commands;

public record DeleteProductCommand(Guid ProductId) : IRequest<Unit>
{
    public ProductId GetProductId() => Domain.Common.ValueObjects.ProductId.Create(ProductId);
}