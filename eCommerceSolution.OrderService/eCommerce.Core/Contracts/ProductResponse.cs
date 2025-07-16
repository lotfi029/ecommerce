namespace eCommerce.Core.Contracts;
public record ProductResponse(
    string Id,
    string Name,
    string Description,
    decimal Price,
    string CategoryId,
    string CategoryName
);
 