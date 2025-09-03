using MediatR;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.ValueObjects;

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
    public CategoryId GetCategoryId() => new(CategoryId);
    public Money GetPrice() => new(Price, Currency);
}