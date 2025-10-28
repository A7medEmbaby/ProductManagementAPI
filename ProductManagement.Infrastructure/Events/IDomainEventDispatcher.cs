namespace ProductManagement.Infrastructure.Events;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync(CancellationToken cancellationToken = default);
}