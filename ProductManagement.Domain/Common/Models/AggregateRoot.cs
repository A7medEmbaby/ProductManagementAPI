namespace ProductManagement.Domain.Common.Models;

public abstract class AggregateRoot<TId, TValue> : Entity<TId>, IHasDomainEvents
    where TId : AggregateRootId<TValue>
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public new TValue Id { get; protected set; }

    protected AggregateRoot(TId id) : base(id)
    {
        Id = id.Value;
    }

    protected AggregateRoot() : base() { }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}