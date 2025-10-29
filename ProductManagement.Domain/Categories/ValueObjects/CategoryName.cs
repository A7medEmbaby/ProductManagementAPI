using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Categories.ValueObjects;

// ⚠️ KEY CHANGE: Inherit from ValueObject instead of record
public sealed class CategoryName : ValueObject
{
    public string Value { get; private set; }

    public CategoryName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Category name cannot be empty", nameof(value));

        var trimmed = value.Trim();
        if (trimmed.Length > 100)
            throw new ArgumentException("Category name cannot exceed 100 characters", nameof(value));

        Value = trimmed;
    }

    private CategoryName() { } // For EF Core

    // ⚠️ NEW: Implement equality based on Value
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    // Keep implicit conversions for convenience
    public static implicit operator string(CategoryName categoryName) => categoryName.Value;
    public static implicit operator CategoryName(string value) => new(value);

    public override string ToString() => Value;
}