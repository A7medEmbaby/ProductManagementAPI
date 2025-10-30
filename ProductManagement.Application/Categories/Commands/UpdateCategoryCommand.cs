using MediatR;
using ProductManagement.Application.Categories.DTOs;
using ProductManagement.Contracts.Categories;
using ProductManagement.Domain.Categories.ValueObjects;

namespace ProductManagement.Application.Categories.Commands;

public record UpdateCategoryCommand(
    Guid CategoryId,
    string Name
) : IRequest<CategoryResponse>
{
    public static UpdateCategoryCommand FromRequest(Guid categoryId, UpdateCategoryRequest request)
        => new(categoryId, request.Name);

    public Domain.Categories.ValueObjects.CategoryId GetCategoryId() => Domain.Categories.ValueObjects.CategoryId.Create(CategoryId);

    public CategoryName GetCategoryName() => new CategoryName(Name);
}