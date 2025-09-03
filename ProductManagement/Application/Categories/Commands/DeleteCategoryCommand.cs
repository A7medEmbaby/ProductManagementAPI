using MediatR;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Application.Categories.Commands;

public record DeleteCategoryCommand(Guid CategoryId) : IRequest<Unit>
{
    public CategoryId GetCategoryId() => new(CategoryId);
}