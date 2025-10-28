using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Domain.Categories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(CategoryId id, CancellationToken cancellationToken = default);
    Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(CategoryId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(CategoryName name, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(CategoryName name, CategoryId excludeId, CancellationToken cancellationToken = default);
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
    Task UpdateAsync(Category category, CancellationToken cancellationToken = default);
    Task DeleteAsync(Category category, CancellationToken cancellationToken = default);
}