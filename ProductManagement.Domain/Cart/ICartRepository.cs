using ProductManagement.Domain.Cart.ValueObjects;

namespace ProductManagement.Application.Cart;

public interface ICartRepository
{
    Task<Domain.Cart.Cart?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);
    Task<Domain.Cart.Cart?> GetByIdAsync(CartId id, CancellationToken cancellationToken = default);
    Task SaveAsync(Domain.Cart.Cart cart, CancellationToken cancellationToken = default);
    Task DeleteAsync(UserId userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(UserId userId, CancellationToken cancellationToken = default);
}