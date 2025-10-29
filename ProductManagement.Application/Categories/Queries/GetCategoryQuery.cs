using MediatR;
using ProductManagement.Application.Categories.DTOs;
using ProductManagement.Domain.Categories.ValueObjects;

namespace ProductManagement.Application.Categories.Queries;

public record GetCategoryQuery(Guid CategoryId) : IRequest<CategoryResponse?>
{
    public Domain.Categories.ValueObjects.CategoryId GetCategoryId() => Domain.Categories.ValueObjects.CategoryId.Create(CategoryId);
}