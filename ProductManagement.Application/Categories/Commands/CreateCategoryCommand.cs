using MediatR;
using ProductManagement.Application.Categories.DTOs;
using ProductManagement.Domain.Categories.ValueObjects;

namespace ProductManagement.Application.Categories.Commands;

public record CreateCategoryCommand(string Name) : IRequest<CategoryResponse>
{
    public static CreateCategoryCommand FromRequest(CreateCategoryRequest request)
        => new(request.Name);

    public CategoryName GetCategoryName() => new(Name);
}