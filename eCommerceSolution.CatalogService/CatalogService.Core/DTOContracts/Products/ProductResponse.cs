namespace CatalogService.Core.DTOContracts.Products;

public record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    Guid? CatagoryId,
    int? Stock,
    string SellerId,
    DateTime CreatedAt
    );