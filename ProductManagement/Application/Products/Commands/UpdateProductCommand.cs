using MediatR;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Application.Products.Commands;

public record UpdateProductCommand(
    Guid ProductId,
    string Name,
    decimal Price,
    string Currency = "USD"
) : IRequest<ProductResponse>
{
    public static UpdateProductCommand FromRequest(Guid productId, UpdateProductRequest request)
        => new(productId, request.Name, request.Price, request.Currency);

    public ProductId GetProductId() => new(ProductId);
    public ProductName GetProductName() => new(Name);
    public Money GetPrice() => new(Price, Currency);
}