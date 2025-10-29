using MediatR;
using ProductManagement.Domain.Categories.ValueObjects;

namespace ProductManagement.Application.Categories.Commands;

public record DeleteCategoryCommand(Guid CategoryId) : IRequest<Unit>
{
    public Domain.Categories.ValueObjects.CategoryId GetCategoryId() => Domain.Categories.ValueObjects.CategoryId.Create(CategoryId);
}