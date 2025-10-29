using MediatR;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Application.Products.Commands;

public record DeleteProductCommand(Guid ProductId) : IRequest<Unit>
{
    public Domain.Products.ValueObjects.ProductId GetProductId() => Domain.Products.ValueObjects.ProductId.Create(ProductId);
}