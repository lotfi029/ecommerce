namespace eCommerce.Core.Contracts;

public record UpdateOrderRequest(
    Guid Id,
    IList<OrderItemRequest> OrderItems
    );
