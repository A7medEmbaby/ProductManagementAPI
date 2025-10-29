using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Categories;
using ProductManagement.Domain.Categories.ValueObjects;

namespace ProductManagement.Infrastructure.Data.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ProductManagementDbContext _context;

    public CategoryRepository(ProductManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(CategoryId id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _context.Categories.ToListAsync(cancellationToken);
        return categories.OrderBy(c => c.Name.Value).ToList();
    }

    public async Task<bool> ExistsAsync(CategoryId id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AnyAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(CategoryName name, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AnyAsync(c => c.Name == name, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(CategoryName name, CategoryId excludeId, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AnyAsync(c => c.Name == name && c.Id != excludeId, cancellationToken);
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddAsync(category, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        _context.Categories.Update(category);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Category category, CancellationToken cancellationToken = default)
    {
        _context.Categories.Remove(category);

        await _context.SaveChangesAsync(cancellationToken);
    }
}