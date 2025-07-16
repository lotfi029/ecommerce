namespace eCommerce.Core.DTO.Products;
public record ProductRequest(
    string Name,
    string Description,
    decimal Price,
    Guid? CategoryId
);
