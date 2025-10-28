using MediatR;
using ProductManagement.Application.Categories.DTOs;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Application.Categories.Commands;

public record UpdateCategoryCommand(
    Guid CategoryId,
    string Name
) : IRequest<CategoryResponse>
{
    public static UpdateCategoryCommand FromRequest(Guid categoryId, UpdateCategoryRequest request)
        => new(categoryId, request.Name);

    public CategoryId GetCategoryId() => new(CategoryId);
    public CategoryName GetCategoryName() => new(Name);
}