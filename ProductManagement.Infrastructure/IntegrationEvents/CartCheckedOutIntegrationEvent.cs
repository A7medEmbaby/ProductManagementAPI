namespace ProductManagement.Application.IntegrationEvents;

public record CartCheckedOutIntegrationEvent
{
    public Guid EventId { get; init; }
    public Guid CartId { get; init; }
    public Guid UserId { get; init; }
    public List<CartItemDto> Items { get; init; } = new();
    public decimal TotalAmount { get; init; }
    public string Currency { get; init; } = "USD";
    public DateTime OccurredAt { get; init; }
}

public record CartItemDto
{
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public string Currency { get; init; } = "USD";
}