namespace ProductManagement.Domain.Common;

public abstract class Entity<TId>
{
    public TId Id { get; protected set; }

    protected Entity(TId id)
    {
        Id = id;
    }

    protected Entity() { }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> entity || entity.GetType() != GetType())
            return false;

        return Id?.Equals(entity.Id) == true;
    }

    public override int GetHashCode()
    {
        return Id?.GetHashCode() ?? 0;
    }
}