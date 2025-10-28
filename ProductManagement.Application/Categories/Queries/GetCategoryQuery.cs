using MediatR;
using ProductManagement.Application.Categories.DTOs;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Application.Categories.Queries;

public record GetCategoryQuery(Guid CategoryId) : IRequest<CategoryResponse?>
{
    public CategoryId GetCategoryId() => new(CategoryId);
}