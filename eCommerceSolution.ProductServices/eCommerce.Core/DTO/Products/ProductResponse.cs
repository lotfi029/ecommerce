using eCommerce.Core.DTO.ProductImage;

namespace eCommerce.Core.DTO.Products;

public record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string CreatedBy,
    Guid? CategoryId,
    bool IsActive,
    DateTime CreatedAt,
    IEnumerable<ProductImageResponse>? Images
);
