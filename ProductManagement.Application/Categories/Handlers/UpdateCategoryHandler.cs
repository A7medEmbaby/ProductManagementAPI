using MediatR;
using ProductManagement.Application.Categories.Commands;
using ProductManagement.Application.Categories.DTOs;
using ProductManagement.Domain.Categories;

namespace ProductManagement.Application.Categories.Handlers;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        // Get category
        var category = await _categoryRepository.GetByIdAsync(request.GetCategoryId(), cancellationToken);
        if (category == null)
            throw new ArgumentException($"Category with ID {request.CategoryId} not found");

        // Check if new name already exists (excluding current category)
        var nameExists = await _categoryRepository.ExistsByNameAsync(request.GetCategoryName(), request.GetCategoryId(), cancellationToken);
        if (nameExists)
            throw new ArgumentException($"Category with name '{request.Name}' already exists");

        // Update category
        category.UpdateName(request.GetCategoryName());

        // Save changes
        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return category.ToResponse();
    }
}