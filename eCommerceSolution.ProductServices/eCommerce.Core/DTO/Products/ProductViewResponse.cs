namespace eCommerce.Core.DTO.Products;

public record ProductViewResponse (
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string CreatedBy,
    bool IsActive,
    Guid? CategoryId,
    DateTime CreatedAt, 
    DateTime? UpdatedAt, 
    Guid? ImageId,
    string? ImageName, 
    DateTime? ImageCreatedAt
    )
{
    public ProductViewResponse() : this(Guid.Empty, string.Empty, string.Empty, 0.0m, string.Empty, false, Guid.Empty, DateTime.MinValue, null, null, null, null)
    {
        
    }
};