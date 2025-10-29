using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Products.ValueObjects;

public sealed class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    public Money(decimal amount, string currency = "USD")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty", nameof(currency));

        Amount = amount;
        Currency = currency.Trim().ToUpperInvariant();
    }

    private Money() { } // For EF Core

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public static Money Zero => new(0);
    public static Money Create(decimal amount, string currency = "USD") => new(amount, currency);

    public override string ToString() => $"{Amount:C} {Currency}";

    // Keep implicit conversion for convenience
    public static implicit operator decimal(Money money) => money.Amount;
}