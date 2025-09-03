using MediatR;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Application.Products.Queries;

public record GetProductsByCategoryQuery(Guid CategoryId) : IRequest<List<ProductResponse>>
{
    public CategoryId GetCategoryId() => new(CategoryId);
}