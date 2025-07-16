namespace eCommerce.Core.Contracts;

public record OrderItemResponse(
    Guid ProductId,
    decimal UnitPrice,
    int Quentity,
    decimal TotalPrice,
    string ProductName,
    string CategoryName
    );