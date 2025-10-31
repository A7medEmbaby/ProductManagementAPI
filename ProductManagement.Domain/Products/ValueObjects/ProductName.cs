using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Products.ValueObjects;

public sealed class ProductName : ValueObject
{
    public string Value { get; private set; }

    public ProductName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Product name cannot be empty", nameof(value));

        var trimmed = value.Trim();
        if (trimmed.Length > 200)
            throw new ArgumentException("Product name cannot exceed 200 characters", nameof(value));

        Value = trimmed;
    }

    private ProductName() { } // For EF Core

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    // Keep implicit conversions for convenience
    public static implicit operator string(ProductName productName) => productName.Value;
    public static implicit operator ProductName(string value) => new(value);

    public override string ToString() => Value;
}