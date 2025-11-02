namespace ProductManagement.Domain.Common.Models;

public abstract class AggregateRoot<TId, TIdType> : Entity<TId>, IHasDomainEvents
    where TId : AggregateRootId<TIdType>
    where TIdType : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public AggregateRootId<TIdType> AggregateId { get; protected set; }

    protected AggregateRoot(TId id) : base(id)
    {
        Id = id;
        this.AggregateId = id;
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