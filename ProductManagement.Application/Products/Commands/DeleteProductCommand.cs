using MediatR;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Application.Products.Commands;

public record DeleteProductCommand(Guid ProductId) : IRequest<Unit>
{
    public ProductId GetProductId() => new(ProductId);
}