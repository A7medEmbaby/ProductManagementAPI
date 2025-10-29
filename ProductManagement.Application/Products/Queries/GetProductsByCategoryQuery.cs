using MediatR;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.Categories.ValueObjects;

namespace ProductManagement.Application.Products.Queries;

public record GetProductsByCategoryQuery(Guid CategoryId) : IRequest<List<ProductResponse>>
{
    public Domain.Categories.ValueObjects.CategoryId GetCategoryId() => Domain.Categories.ValueObjects.CategoryId.Create(CategoryId);
}