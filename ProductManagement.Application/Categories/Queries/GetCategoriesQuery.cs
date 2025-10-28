using MediatR;
using ProductManagement.Application.Categories.DTOs;

namespace ProductManagement.Application.Categories.Queries;

public record GetCategoriesQuery() : IRequest<List<CategoryResponse>>;