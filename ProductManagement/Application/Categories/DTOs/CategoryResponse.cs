namespace ProductManagement.Application.Categories.DTOs;

public record CategoryResponse(
    Guid Id,
    string Name,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public static class CategoryExtensions
{
    public static CategoryResponse ToResponse(this Domain.Categories.Category category)
        => new(
            category.Id.Value,
            category.Name.Value,
            category.CreatedAt,
            category.UpdatedAt
        );

    public static List<CategoryResponse> ToResponse(this IEnumerable<Domain.Categories.Category> categories)
        => categories.Select(ToResponse).ToList();
}