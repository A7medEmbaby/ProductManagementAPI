using MediatR;
using ProductManagement.Domain.Common;
using ProductManagement.Infrastructure.Data;

namespace ProductManagement.Infrastructure.Events;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly ProductManagementDbContext _context;

    public DomainEventDispatcher(IMediator mediator, ProductManagementDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task DispatchEventsAsync(CancellationToken cancellationToken = default)
    {
        // Use the SAME context that has the tracked entities
        var productEntities = _context.ChangeTracker.Entries<ProductManagement.Domain.Products.Product>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var categoryEntities = _context.ChangeTracker.Entries<ProductManagement.Domain.Categories.Category>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        // Collect all domain events
        var domainEvents = new List<IDomainEvent>();

        foreach (var product in productEntities)
        {
            domainEvents.AddRange(product.DomainEvents);
            product.ClearDomainEvents();
        }

        foreach (var category in categoryEntities)
        {
            domainEvents.AddRange(category.DomainEvents);
            category.ClearDomainEvents();
        }

        // Debug logging
        Console.WriteLine($"Found {productEntities.Count} products with events");
        Console.WriteLine($"Found {categoryEntities.Count} categories with events");
        Console.WriteLine($"Total domain events to dispatch: {domainEvents.Count}");

        // Publish each event
        foreach (var domainEvent in domainEvents)
        {
            Console.WriteLine($"Publishing event: {domainEvent.GetType().Name}");
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}