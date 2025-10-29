using MediatR;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.Categories.ValueObjects;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Application.Products.Commands;

public record CreateProductCommand(
    string Name,
    Guid CategoryId,
    decimal Price,
    string Currency = "USD"
) : IRequest<ProductResponse>
{
    public static CreateProductCommand FromRequest(CreateProductRequest request)
        => new(request.Name, request.CategoryId, request.Price, request.Currency);

    public ProductName GetProductName() => new(Name);
    public Domain.Categories.ValueObjects.CategoryId GetCategoryId() => Domain.Categories.ValueObjects.CategoryId.Create(CategoryId);
    public Money GetPrice() => new(Price, Currency);
}