namespace ProductManagement.Application.Products.DTOs;

public record ProductResponse(
    Guid Id,
    string Name,
    Guid CategoryId,
    decimal Price,
    string Currency,
    int StockQuantity,
    int ReservedQuantity,
    int AvailableQuantity,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public static class ProductExtensions
{
    public static ProductResponse ToResponse(this Domain.Products.Product product)
        => new(
            product.Id.Value,
            product.Name.Value,
            product.CategoryId.Value,
            product.Price.Amount,
            product.Price.Currency,
            product.Stock.Quantity,
            product.Stock.ReservedQuantity,
            product.Stock.AvailableQuantity,
            product.CreatedAt,
            product.UpdatedAt
        );

    public static List<ProductResponse> ToResponse(this IEnumerable<Domain.Products.Product> products)
        => products.Select(ToResponse).ToList();
}