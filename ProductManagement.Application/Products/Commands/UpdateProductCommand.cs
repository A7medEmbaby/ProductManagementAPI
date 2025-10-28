using MediatR;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Application.Products.Commands;

public record UpdateProductCommand(
    Guid ProductId,
    string Name,
    Guid CategoryId,
    decimal Price,
    string Currency = "USD"
) : IRequest<ProductResponse>
{
    public static UpdateProductCommand FromRequest(Guid productId, UpdateProductRequest request)
        => new(productId, request.Name, request.CategoryId, request.Price, request.Currency);

    public ProductId GetProductId() => new(ProductId);
    public ProductName GetProductName() => new(Name);
    public CategoryId GetCategoryId() => new(CategoryId);
    public Money GetPrice() => new(Price, Currency);
}