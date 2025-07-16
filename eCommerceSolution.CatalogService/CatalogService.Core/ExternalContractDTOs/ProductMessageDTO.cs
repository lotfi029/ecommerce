namespace eCommerceCatalogService.Core.ExternalContractDTOs;
public record ProductMessageDTO(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    Guid? CategoryId,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string UpdatedBy,
    ICollection<ProductImageMessageDTO>? Images
    );