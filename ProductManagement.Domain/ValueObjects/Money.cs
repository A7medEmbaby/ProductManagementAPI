namespace ProductManagement.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "USD")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty", nameof(currency));

        Amount = amount;
        Currency = currency.Trim().ToUpperInvariant();
    }

    public static Money Zero => new(0);
    public static Money Create(decimal amount, string currency = "USD") => new(amount, currency);

    public override string ToString() => $"{Amount:C} {Currency}";

    public static implicit operator decimal(Money money) => money.Amount;
}