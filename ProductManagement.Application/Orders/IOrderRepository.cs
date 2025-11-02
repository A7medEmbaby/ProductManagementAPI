using ProductManagement.Domain.Cart.ValueObjects;
using ProductManagement.Domain.Orders.ValueObjects;

namespace ProductManagement.Application.Orders;

public interface IOrderRepository
{
    Task<Domain.Orders.Order?> GetByIdAsync(OrderId id, CancellationToken cancellationToken = default);
    Task<List<Domain.Orders.Order>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Domain.Orders.Order>> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);
    Task<List<Domain.Orders.Order>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<List<Domain.Orders.Order>> GetPagedByUserIdAsync(UserId userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetTotalCountByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(OrderId id, CancellationToken cancellationToken = default);
    Task AddAsync(Domain.Orders.Order order, CancellationToken cancellationToken = default);
    Task UpdateAsync(Domain.Orders.Order order, CancellationToken cancellationToken = default);
}