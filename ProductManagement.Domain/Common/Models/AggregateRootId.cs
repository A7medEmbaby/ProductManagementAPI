namespace ProductManagement.Domain.Common.Models;

public abstract class AggregateRootId<TValue> : ValueObject
{
    public abstract TValue Value { get; protected set; }

    protected AggregateRootId() { } // For EF Core

    public override string ToString() => Value?.ToString() ?? string.Empty;
}