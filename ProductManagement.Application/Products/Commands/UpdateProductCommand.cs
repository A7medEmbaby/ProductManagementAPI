using MediatR;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.Categories.ValueObjects;
using ProductManagement.Domain.Products.ValueObjects;
using ProductManagement.Contracts.Products;

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

    public Domain.Products.ValueObjects.ProductId GetProductId() => Domain.Products.ValueObjects.ProductId.Create(ProductId);
    public ProductName GetProductName() => new(Name);
    public Domain.Categories.ValueObjects.CategoryId GetCategoryId() => Domain.Categories.ValueObjects.CategoryId.Create(CategoryId);
    public Money GetPrice() => new(Price, Currency);
}