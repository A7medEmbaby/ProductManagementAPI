using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Products;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Infrastructure.Events;

namespace ProductManagement.Infrastructure.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductManagementDbContext _context;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public ProductRepository(ProductManagementDbContext context, IDomainEventDispatcher domainEventDispatcher)
    {
        _context = context;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<Product?> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await _context.Products.ToListAsync(cancellationToken);
        return products.OrderBy(p => p.Name.Value).ToList();
    }

    public async Task<List<Product>> GetByCategoryIdAsync(CategoryId categoryId, CancellationToken cancellationToken = default)
    {
        var products = await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
        return products.OrderBy(p => p.Name.Value).ToList();
    }

    public async Task<List<Product>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var products = await _context.Products.ToListAsync(cancellationToken);
        return products
            .OrderBy(p => p.Name.Value)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(ProductId id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AnyAsync(p => p.Id == id, cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        await _domainEventDispatcher.DispatchEventsAsync(cancellationToken);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
        await _domainEventDispatcher.DispatchEventsAsync(cancellationToken);
    }

    public async Task DeleteAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
        await _domainEventDispatcher.DispatchEventsAsync(cancellationToken);
    }
}