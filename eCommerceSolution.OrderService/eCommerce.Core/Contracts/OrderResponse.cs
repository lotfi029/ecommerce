namespace eCommerce.Core.Contracts;

public record OrderResponse(
    Guid Id,
    Guid UserId,
    DateTime CreatedAt,
    decimal TotalBill,
    IList<OrderItemResponse> OrderItems
    );



