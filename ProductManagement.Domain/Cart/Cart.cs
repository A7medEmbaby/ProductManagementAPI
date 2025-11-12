using ProductManagement.Domain.Cart.Events;
using ProductManagement.Domain.Cart.ValueObjects;
using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Common.ValueObjects;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Domain.Cart;

public class Cart : AggregateRoot<CartId, Guid>
{
    private readonly List<CartItem> _items = new();

    public UserId UserId { get; private set; }
    public CartStatus Status { get; private set; }
    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
    public int ItemCount => _items.Sum(i => i.Quantity);
    public Money TotalAmount
    {
        get
        {
            if (!_items.Any())
                return Money.Zero;

            var currency = _items.First().UnitPrice.Currency;
            var total = _items.Sum(i => i.LineTotal.Amount);
            return Money.Create(total, currency);
        }
    }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Cart() : base() { } // For serialization

    private Cart(CartId id, UserId userId) : base(id)
    {
        UserId = userId;
        Status = CartStatus.Active;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public static Cart Create(UserId userId)
    {
        var cartId = CartId.CreateUnique();
        var cart = new Cart(cartId, userId);
        return cart;
    }

    public void AddItem(ProductId productId, ProductName productName, int quantity, Money unitPrice)
    {
        if (Status == CartStatus.CheckedOut)
            throw new InvalidOperationException("Cannot add items to a checked-out cart");

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        // Check if item already exists
        var existingItem = _items.FirstOrDefault(i => i.ProductId.Value == productId.Value);

        if (existingItem != null)
        {
            // Update quantity of existing item
            existingItem.IncreaseQuantity(quantity);
            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(CartItemQuantityUpdatedEvent.Create(
                (CartId)AggregateId,
                UserId,
                existingItem.Id,
                productId,
                existingItem.Quantity - quantity,
                existingItem.Quantity
            ));
        }
        else
        {
            // Add new item
            var newItem = CartItem.Create(productId, productName, quantity, unitPrice);
            _items.Add(newItem);
            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(CartItemAddedEvent.Create(
                (CartId)AggregateId,
                UserId,
                productId,
                productName,
                quantity,
                unitPrice
            ));
        }
    }

    public void RemoveItem(CartItemId itemId)
    {
        if (Status == CartStatus.CheckedOut)
            throw new InvalidOperationException("Cannot remove items from a checked-out cart");

        var item = _items.FirstOrDefault(i => i.Id.Value == itemId.Value);

        if (item == null)
            throw new ArgumentException($"Cart item with ID {itemId} not found");

        // Capture item details before removing
        var productId = item.ProductId;
        var quantity = item.Quantity;

        _items.Remove(item);
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(CartItemRemovedEvent.Create(
            (CartId)AggregateId,
            UserId,
            itemId,
            productId,
            quantity
        ));
    }

    public void UpdateItemQuantity(CartItemId itemId, int newQuantity)
    {
        if (Status == CartStatus.CheckedOut)
            throw new InvalidOperationException("Cannot update item quantities in a checked-out cart");

        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(newQuantity));

        var item = _items.FirstOrDefault(i => i.Id.Value == itemId.Value);

        if (item == null)
            throw new ArgumentException($"Cart item with ID {itemId} not found");

        var oldQuantity = item.Quantity;
        var productId = item.ProductId;
        item.UpdateQuantity(newQuantity);
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(CartItemQuantityUpdatedEvent.Create(
            (CartId)AggregateId,
            UserId,
            itemId,
            productId,
            oldQuantity,
            newQuantity
        ));
    }

    public void Clear()
    {
        // Capture items before clearing
        var clearedItems = _items.Select(item => new ClearedCartItem(
            item.ProductId,
            item.Quantity
        )).ToList();

        _items.Clear();
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(CartClearedEvent.Create(
            (CartId)AggregateId,
            UserId,
            clearedItems
        ));
    }

    public void Checkout()
    {
        if (Status == CartStatus.CheckedOut)
            throw new InvalidOperationException("Cart has already been checked out");

        if (!_items.Any())
            throw new InvalidOperationException("Cannot checkout an empty cart");

        var checkoutItems = _items.Select(item => new CartCheckoutItem(
            item.ProductId,
            item.ProductName,
            item.Quantity,
            item.UnitPrice
        )).ToList();

        // Mark cart as checked out
        MarkAsCheckedOut();

        RaiseDomainEvent(CartCheckedOutEvent.Create(
            (CartId)AggregateId,
            UserId,
            checkoutItems,
            TotalAmount
        ));
    }

    private void MarkAsCheckedOut()
    {
        Status = CartStatus.CheckedOut;
        UpdatedAt = DateTime.UtcNow;
    }

    public CartItem? GetItem(CartItemId itemId)
    {
        return _items.FirstOrDefault(i => i.Id.Value == itemId.Value);
    }

    public bool HasItem(ProductId productId)
    {
        return _items.Any(i => i.ProductId.Value == productId.Value);
    }
}