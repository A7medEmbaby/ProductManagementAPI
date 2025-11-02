using ProductManagement.Domain.Categories.ValueObjects;
using ProductManagement.Domain.Categories.Events;
using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Categories;

public class Category : AggregateRoot<CategoryId, Guid>
{
    public CategoryName Name { get; private set; }
    public int ProductCount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Category() : base() { } // For EF Core

    private Category(CategoryId id, CategoryName name) : base(id)
    {
        Name = name;
        ProductCount = 0;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public static Category Create(CategoryName name)
    {
        var categoryId = CategoryId.CreateUnique();
        var category = new Category(categoryId, name);

        category.RaiseDomainEvent(CategoryCreatedEvent.Create(categoryId, name));

        return category;
    }

    public void UpdateName(CategoryName newName)
    {
        if (Name.Value == newName.Value) return;

        var oldName = Name;
        Name = newName;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(CategoryUpdatedEvent.Create(
            (CategoryId)AggregateId,
            oldName,
            newName));
    }

    public void IncrementProductCount()
    {
        ProductCount++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void DecrementProductCount()
    {
        if (ProductCount > 0)
        {
            ProductCount--;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void Delete()
    {
        RaiseDomainEvent(CategoryDeletedEvent.Create(
            (CategoryId)AggregateId,
            Name));
    }
}