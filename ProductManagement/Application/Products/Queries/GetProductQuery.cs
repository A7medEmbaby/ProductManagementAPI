using MediatR;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Application.Products.Queries;

public record GetProductQuery(Guid ProductId) : IRequest<ProductResponse?>
{
    public ProductId GetProductId() => new(ProductId);
}