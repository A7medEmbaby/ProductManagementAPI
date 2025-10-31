using ProductManagement.Domain.Products.ValueObjects;
using ProductManagement.Domain.Categories.ValueObjects;
using ProductManagement.Domain.Products.Events;
using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Products;

public class Product : AggregateRoot<ProductId, Guid>
{
    public ProductName Name { get; private set; }
    public CategoryId CategoryId { get; private set; }
    public Money Price { get; private set; }
    public Stock Stock { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Product() : base() { } // For EF Core

    private Product(ProductId id, ProductName name, CategoryId categoryId, Money price, Stock stock)
        : base(id)
    {
        Name = name;
        CategoryId = categoryId;
        Price = price;
        Stock = stock;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public static Product Create(ProductName name, CategoryId categoryId, Money price, int initialStock = 0)
    {
        var productId = ProductId.CreateUnique();
        var stock = Stock.Create(initialStock);
        var product = new Product(productId, name, categoryId, price, stock);

        product.RaiseDomainEvent(ProductCreatedEvent.Create(productId, name, categoryId, price));

        return product;
    }

    public void UpdateName(ProductName newName)
    {
        if (Name.Value == newName.Value) return;

        Name = newName;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(ProductUpdatedEvent.Create(
            ProductId.Create(((AggregateRootId<Guid>)Id).Value),
            Name,
            Price));
    }

    public void UpdatePrice(Money newPrice)
    {
        if (Price.Amount == newPrice.Amount && Price.Currency == newPrice.Currency) return;

        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(ProductUpdatedEvent.Create(
            ProductId.Create(((AggregateRootId<Guid>)Id).Value),
            Name,
            Price));
    }

    public void ChangeCategory(CategoryId newCategoryId)
    {
        if (CategoryId.Value == newCategoryId.Value) return;

        var oldCategoryId = CategoryId;
        CategoryId = newCategoryId;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(ProductCategoryChangedEvent.Create(
            ProductId.Create(((AggregateRootId<Guid>)Id).Value),
            oldCategoryId,
            newCategoryId));
    }

    public void ReserveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to reserve must be greater than zero", nameof(quantity));

        Stock = Stock.Reserve(quantity);
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(StockReservedEvent.Create(
            ProductId.Create(((AggregateRootId<Guid>)Id).Value),
            quantity,
            Stock.AvailableQuantity));
    }

    public void ReleaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to release must be greater than zero", nameof(quantity));

        Stock = Stock.Release(quantity);
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(StockReleasedEvent.Create(
            ProductId.Create(((AggregateRootId<Guid>)Id).Value),
            quantity,
            Stock.AvailableQuantity));
    }

    public void DeductStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to deduct must be greater than zero", nameof(quantity));

        Stock = Stock.Deduct(quantity);
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(StockDeductedEvent.Create(
            ProductId.Create(((AggregateRootId<Guid>)Id).Value),
            quantity,
            Stock.Quantity));
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to add must be greater than zero", nameof(quantity));

        Stock = Stock.Add(quantity);
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(StockAddedEvent.Create(
            ProductId.Create(((AggregateRootId<Guid>)Id).Value),
            quantity,
            Stock.Quantity));
    }

    public void UpdateStock(int newQuantity)
    {
        if (newQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative", nameof(newQuantity));

        var oldQuantity = Stock.Quantity;
        Stock = Stock.Update(newQuantity);
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(StockUpdatedEvent.Create(
            ProductId.Create(((AggregateRootId<Guid>)Id).Value),
            oldQuantity,
            newQuantity));
    }

    public bool HasAvailableStock(int quantity)
    {
        return Stock.HasAvailableStock(quantity);
    }

    public void Delete()
    {
        RaiseDomainEvent(ProductDeletedEvent.Create(
            ProductId.Create(((AggregateRootId<Guid>)Id).Value),
            CategoryId));
    }
}