namespace ProductManagement.Domain.Common.Models;

public abstract class AggregateRootId<TValue> : ValueObject
{
    public TValue Value { get; protected set; }

    protected AggregateRootId(TValue value)
    {
        Value = value;
    }

    protected AggregateRootId() { } // For EF Core

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value?.ToString() ?? string.Empty;
}