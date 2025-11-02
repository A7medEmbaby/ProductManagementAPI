using ProductManagement.Domain.Cart.ValueObjects;
using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Common.ValueObjects;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Domain.Cart;

public class CartItem : Entity<CartItemId>
{
    public ProductId ProductId { get; private set; }
    public ProductName ProductName { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public Money LineTotal => Money.Create(UnitPrice.Amount * Quantity, UnitPrice.Currency);

    private CartItem(CartItemId id, ProductId productId, ProductName productName, int quantity, Money unitPrice)
        : base(id)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    private CartItem() : base() { } // For serialization

    public static CartItem Create(ProductId productId, ProductName productName, int quantity, Money unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        var itemId = CartItemId.CreateUnique();
        return new CartItem(itemId, productId, productName, quantity, unitPrice);
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(newQuantity));

        Quantity = newQuantity;
    }

    public void IncreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero", nameof(amount));

        Quantity += amount;
    }
}