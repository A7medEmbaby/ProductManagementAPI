using System.Collections.Concurrent;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.Cart;
using ProductManagement.Domain.Cart.ValueObjects;

namespace ProductManagement.Infrastructure.Repositories;

public class InMemoryCartRepository : ICartRepository
{
    private readonly ConcurrentDictionary<Guid, Domain.Cart.Cart> _carts = new();
    private readonly IServiceProvider _serviceProvider;

    public InMemoryCartRepository(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<Domain.Cart.Cart?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        _carts.TryGetValue(userId.Value, out var cart);
        return Task.FromResult(cart);
    }

    public Task<Domain.Cart.Cart?> GetByIdAsync(CartId id, CancellationToken cancellationToken = default)
    {
        var cart = _carts.Values.FirstOrDefault(c => ((CartId)c.AggregateId).Value == id.Value);
        return Task.FromResult(cart);
    }

    public async Task SaveAsync(Domain.Cart.Cart cart, CancellationToken cancellationToken = default)
    {
        // Get domain events before saving
        var domainEvents = cart.DomainEvents.ToList();

        // Clear domain events from the cart
        cart.ClearDomainEvents();

        // Save to in-memory store
        _carts.AddOrUpdate(cart.UserId.Value, cart, (key, oldValue) => cart);

        // Publish domain events using a scoped service provider
        using var scope = _serviceProvider.CreateScope();
        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }
    }

    public Task DeleteAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        _carts.TryRemove(userId.Value, out _);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_carts.ContainsKey(userId.Value));
    }
}