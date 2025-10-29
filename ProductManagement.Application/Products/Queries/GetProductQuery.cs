using MediatR;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Application.Products.Queries;

public record GetProductQuery(Guid ProductId) : IRequest<ProductResponse?>
{
    public Domain.Products.ValueObjects.ProductId GetProductId() => Domain.Products.ValueObjects.ProductId.Create(ProductId);
}