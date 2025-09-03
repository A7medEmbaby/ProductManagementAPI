using MediatR;
using ProductManagement.Domain.Common;
using ProductManagement.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ProductManagement.Infrastructure.Events;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IMediator mediator, IServiceProvider serviceProvider)
    {
        _mediator = mediator;
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchEventsAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProductManagementDbContext>();

        // Get all entities with domain events
        var entitiesWithEvents = context.ChangeTracker.Entries()
            .Where(e => e.Entity is AggregateRoot<object> && ((AggregateRoot<object>)e.Entity).DomainEvents.Any())
            .Select(e => (AggregateRoot<object>)e.Entity)
            .ToList();

        // Collect all domain events
        var domainEvents = entitiesWithEvents
            .SelectMany(e => e.DomainEvents)
            .ToList();

        // Clear events from entities first
        entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

        // Publish each event
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}