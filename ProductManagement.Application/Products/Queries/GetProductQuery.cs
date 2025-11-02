using MediatR;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.Common.ValueObjects;

namespace ProductManagement.Application.Products.Queries;

public record GetProductQuery(Guid ProductId) : IRequest<ProductResponse?>
{
    public ProductId GetProductId() => Domain.Common.ValueObjects.ProductId.Create(ProductId);
}