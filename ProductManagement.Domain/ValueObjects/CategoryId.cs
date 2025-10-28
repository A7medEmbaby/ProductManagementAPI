namespace ProductManagement.Domain.ValueObjects;

public record CategoryId(Guid Value)
{
    public static CategoryId New() => new(Guid.NewGuid());
    public static CategoryId Empty => new(Guid.Empty);

    public static implicit operator Guid(CategoryId categoryId) => categoryId.Value;
    public static implicit operator CategoryId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}