namespace eCommerce.Core.Contracts;
public record CreateOrderRequest(
    IList<OrderItemRequest> OrderItems
    );
