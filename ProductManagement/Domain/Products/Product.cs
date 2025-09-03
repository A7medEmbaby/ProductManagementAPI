using ProductManagement.Domain.Common;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Domain.Products.Events;

namespace ProductManagement.Domain.Products;

public class Product : AggregateRoot<ProductId>
{
    public ProductName Name { get; private set; }
    public CategoryId CategoryId { get; private set; }
    public Money Price { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Product() : base() { }

    private Product(ProductId id, ProductName name, CategoryId categoryId, Money price)
        : base(id)
    {
        Name = name;
        CategoryId = categoryId;
        Price = price;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public static Product Create(ProductName name, CategoryId categoryId, Money price)
    {
        var productId = ProductId.New();
        var product = new Product(productId, name, categoryId, price);

        product.RaiseDomainEvent(ProductCreatedEvent.Create(productId, name, categoryId, price));

        return product;
    }

    public void UpdateName(ProductName newName)
    {
        if (Name.Value == newName.Value) return;

        Name = newName;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(ProductUpdatedEvent.Create(Id, Name, Price));
    }

    public void UpdatePrice(Money newPrice)
    {
        if (Price.Amount == newPrice.Amount && Price.Currency == newPrice.Currency) return;

        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(ProductUpdatedEvent.Create(Id, Name, Price));
    }

    public void ChangeCategory(CategoryId newCategoryId)
    {
        if (CategoryId.Value == newCategoryId.Value) return;

        var oldCategoryId = CategoryId;
        CategoryId = newCategoryId;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(ProductCategoryChangedEvent.Create(Id, oldCategoryId, newCategoryId));
    }

    public void Delete()
    {
        RaiseDomainEvent(ProductDeletedEvent.Create(Id, CategoryId));
    }
}