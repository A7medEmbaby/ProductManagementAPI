using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Products.ValueObjects;

public sealed class Stock : ValueObject
{
    public int Quantity { get; private set; }
    public int ReservedQuantity { get; private set; }
    public int AvailableQuantity => Quantity - ReservedQuantity;

    public Stock(int quantity, int reservedQuantity = 0)
    {
        if (quantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative", nameof(quantity));

        if (reservedQuantity < 0)
            throw new ArgumentException("Reserved quantity cannot be negative", nameof(reservedQuantity));

        if (reservedQuantity > quantity)
            throw new ArgumentException("Reserved quantity cannot exceed total quantity", nameof(reservedQuantity));

        Quantity = quantity;
        ReservedQuantity = reservedQuantity;
    }

    private Stock() { } // For EF Core

    public static Stock Create(int quantity) => new(quantity, 0);

    public static Stock Zero => new(0, 0);

    public bool HasAvailableStock(int requestedQuantity)
    {
        return AvailableQuantity >= requestedQuantity;
    }

    public Stock Reserve(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to reserve must be greater than zero", nameof(quantity));

        if (!HasAvailableStock(quantity))
            throw new InvalidOperationException($"Insufficient available stock. Available: {AvailableQuantity}, Requested: {quantity}");

        return new Stock(Quantity, ReservedQuantity + quantity);
    }

    public Stock Release(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to release must be greater than zero", nameof(quantity));

        if (quantity > ReservedQuantity)
            throw new InvalidOperationException($"Cannot release more than reserved. Reserved: {ReservedQuantity}, Requested: {quantity}");

        return new Stock(Quantity, ReservedQuantity - quantity);
    }

    public Stock Deduct(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to deduct must be greater than zero", nameof(quantity));

        if (quantity > Quantity)
            throw new InvalidOperationException($"Cannot deduct more than available quantity. Available: {Quantity}, Requested: {quantity}");

        // If we have reserved quantity, deduct from both total and reserved
        var newReservedQuantity = Math.Max(0, ReservedQuantity - quantity);
        return new Stock(Quantity - quantity, newReservedQuantity);
    }

    public Stock Add(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to add must be greater than zero", nameof(quantity));

        return new Stock(Quantity + quantity, ReservedQuantity);
    }

    public Stock Update(int newQuantity)
    {
        if (newQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative", nameof(newQuantity));

        if (newQuantity < ReservedQuantity)
            throw new InvalidOperationException($"Cannot set quantity below reserved amount. Reserved: {ReservedQuantity}, New Quantity: {newQuantity}");

        return new Stock(newQuantity, ReservedQuantity);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Quantity;
        yield return ReservedQuantity;
    }

    public override string ToString() => $"Total: {Quantity}, Reserved: {ReservedQuantity}, Available: {AvailableQuantity}";
}