namespace eCommerce.Core.IRabbitMQ.Contracts;
public record ProductMessageDTO(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    Guid? CategoryId,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string UpdatedBy
    );