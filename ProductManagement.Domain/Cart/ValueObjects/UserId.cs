using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Cart.ValueObjects;

public sealed class UserId : ValueObject
{
    public Guid Value { get; private set; }

    public UserId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty", nameof(value));

        Value = value;
    }

    private UserId() { } // For EF Core

    public static UserId Create(Guid value) => new(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator Guid(UserId userId) => userId.Value;
    public static implicit operator UserId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}