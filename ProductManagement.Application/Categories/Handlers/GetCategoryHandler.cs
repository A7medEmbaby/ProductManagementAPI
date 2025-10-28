using MediatR;
using ProductManagement.Application.Categories.Queries;
using ProductManagement.Application.Categories.DTOs;
using ProductManagement.Domain.Categories;

namespace ProductManagement.Application.Categories.Handlers;

public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, CategoryResponse?>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryResponse?> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.GetCategoryId(), cancellationToken);
        return category?.ToResponse();
    }
}