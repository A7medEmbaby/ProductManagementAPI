namespace ProductManagement.Domain.ValueObjects;

public record CategoryName
{
    public string Value { get; }

    public CategoryName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Category name cannot be empty", nameof(value));

        var trimmed = value.Trim();
        if (trimmed.Length > 100)
            throw new ArgumentException("Category name cannot exceed 100 characters", nameof(value));

        Value = trimmed;
    }

    public static implicit operator string(CategoryName categoryName) => categoryName.Value;
    public static implicit operator CategoryName(string value) => new(value);

    public override string ToString() => Value;
}