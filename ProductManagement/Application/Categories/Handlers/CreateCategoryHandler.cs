using MediatR;
using ProductManagement.Application.Categories.Commands;
using ProductManagement.Application.Categories.DTOs;
using ProductManagement.Domain.Categories;

namespace ProductManagement.Application.Categories.Handlers;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        // Check if category name already exists
        var nameExists = await _categoryRepository.ExistsByNameAsync(request.GetCategoryName(), cancellationToken);
        if (nameExists)
            throw new ArgumentException($"Category with name '{request.Name}' already exists");

        // Create category
        var category = Category.Create(request.GetCategoryName());

        // Save category
        await _categoryRepository.AddAsync(category, cancellationToken);

        return category.ToResponse();
    }
}