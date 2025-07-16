using eCommerce.Core.Abstractions;
using eCommerce.Core.Contracts;
using eCommerce.Core.Entities;
using MongoDB.Driver;

namespace eCommerce.Core.ServiceContracts;
public interface IOrdersService
{
    Task<Result<Guid>> AddOrderAsync(Guid userId, CreateOrderRequest request, CancellationToken ct = default);
    Task<Result> UpdateOrderRequest(Guid userId, UpdateOrderRequest request, CancellationToken ct = default);
    Task<Result> DeleteOrderAsync(Guid Id);
    Task<Result<OrderResponse>> GetOrderByConditionAsync(FilterDefinition<Order> filter, CancellationToken ct = default);
    Task<Result<IEnumerable<OrderResponse>>> GetOrdersByConditionAsync(FilterDefinition<Order> filter, CancellationToken ct = default);
    Task<Result<IEnumerable<OrderResponse>>> GetOrdersAsync(CancellationToken ct = default);
}
