using ProductManagement.Domain.Cart.ValueObjects;
using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Orders.Events;
using ProductManagement.Domain.Orders.ValueObjects;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Domain.Orders;

public class Order : AggregateRoot<OrderId, Guid>
{
    private readonly List<OrderItem> _items = new();

    public UserId UserId { get; private set; }
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public OrderStatus Status { get; private set; }
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
    public DateTime? CompletedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    private Order() : base() { } // For EF Core

    private Order(OrderId id, UserId userId) : base(id)
    {
        UserId = userId;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public static Order Create(UserId userId, List<OrderItemData> items)
    {
        if (items == null || !items.Any())
            throw new ArgumentException("Order must have at least one item", nameof(items));

        var orderId = OrderId.CreateUnique();
        var order = new Order(orderId, userId);

        // Add items
        foreach (var itemData in items)
        {
            var orderItem = OrderItem.Create(
                itemData.ProductId,
                itemData.ProductName,
                itemData.Quantity,
                itemData.UnitPrice
            );
            order._items.Add(orderItem);
        }

        // Raise domain event
        order.RaiseDomainEvent(OrderCreatedEvent.Create(orderId, userId, items, order.TotalAmount));

        return order;
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Cannot confirm order with status {Status}");

        Status = OrderStatus.Confirmed;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(OrderConfirmedEvent.Create(
            (OrderId)AggregateId,
            UserId
        ));
    }

    public void Process()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException($"Cannot process order with status {Status}");

        Status = OrderStatus.Processing;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status != OrderStatus.Processing && Status != OrderStatus.Confirmed)
            throw new InvalidOperationException($"Cannot complete order with status {Status}");

        Status = OrderStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
        CompletedAt = DateTime.UtcNow;

        RaiseDomainEvent(OrderCompletedEvent.Create(
            (OrderId)AggregateId,
            UserId
        ));
    }

    public void Cancel(string reason)
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed order");

        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Order is already cancelled");

        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
        CancelledAt = DateTime.UtcNow;

        RaiseDomainEvent(OrderCancelledEvent.Create(
            (OrderId)AggregateId,
            UserId,
            reason
        ));
    }

    public void MarkAsFailed(string reason)
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Cannot mark a completed order as failed");

        Status = OrderStatus.Failed;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(OrderFailedEvent.Create(
            (OrderId)AggregateId,
            UserId,
            reason
        ));
    }

    public bool CanBeCancelled()
    {
        return Status != OrderStatus.Completed && Status != OrderStatus.Cancelled;
    }
}