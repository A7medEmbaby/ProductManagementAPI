using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Domain.Products;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default);
    Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Product>> GetByCategoryIdAsync(CategoryId categoryId, CancellationToken cancellationToken = default);
    Task<List<Product>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(ProductId id, CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task DeleteAsync(Product product, CancellationToken cancellationToken = default);
}