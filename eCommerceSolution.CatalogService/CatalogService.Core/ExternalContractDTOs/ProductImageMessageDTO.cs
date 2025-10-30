namespace CatalogService.Core.ExternalContractDTOs;
public record ProductImageMessageDTO(
    Guid Id,
    string Name,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string UpdatedBy
    );
