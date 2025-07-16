namespace eCommerce.Core.Contracts;

public record OrderItemRequest(
    Guid ProductId,
    decimal UnitPrice,
    int Quantity
    );
