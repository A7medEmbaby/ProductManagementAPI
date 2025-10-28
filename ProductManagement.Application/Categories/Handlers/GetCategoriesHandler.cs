using MediatR;
using ProductManagement.Application.Categories.Queries;
using ProductManagement.Application.Categories.DTOs;
using ProductManagement.Domain.Categories;

namespace ProductManagement.Application.Categories.Handlers;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return categories.ToResponse();
    }
}