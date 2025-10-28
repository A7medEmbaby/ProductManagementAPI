namespace ProductManagement.Domain.ValueObjects;

public record ProductName
{
    public string Value { get; }

    public ProductName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Product name cannot be empty", nameof(value));

        var trimmed = value.Trim();
        if (trimmed.Length > 200)
            throw new ArgumentException("Product name cannot exceed 200 characters", nameof(value));

        Value = trimmed;
    }

    public static implicit operator string(ProductName productName) => productName.Value;
    public static implicit operator ProductName(string value) => new(value);

    public override string ToString() => Value;
}