namespace ProductManagement.Domain.Common.Models;

public abstract class AggregateRootId<TValue> : ValueObject
{
    public abstract TValue Value { get; protected set; }

    protected AggregateRootId() { } // For EF Core

    /// <summary>
    /// Implements equality comparison based on the Value property.
    /// All derived IDs are equal if their Value is equal.
    /// </summary>
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value?.ToString() ?? string.Empty;
}