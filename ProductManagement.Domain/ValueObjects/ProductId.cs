namespace ProductManagement.Domain.ValueObjects;

public record ProductId(Guid Value)
{
    public static ProductId New() => new(Guid.NewGuid());
    public static ProductId Empty => new(Guid.Empty);

    public static implicit operator Guid(ProductId productId) => productId.Value;
    public static implicit operator ProductId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}