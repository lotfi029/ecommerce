namespace eCommerce.Core.DTO.Categories;

public record CategoryResponse(
    string Id,
    string Name,
    string CreatedBy,
    string Description
);