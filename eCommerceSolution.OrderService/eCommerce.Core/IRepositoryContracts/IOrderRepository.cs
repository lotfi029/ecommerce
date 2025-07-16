using eCommerce.Core.Entities;
using MongoDB.Driver;

namespace eCommerce.Core.IRepositoryContracts;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrders(CancellationToken cancellationToken = default);
    Task<IEnumerable<Order?>> GetOrdersByCondition(FilterDefinition<Order> filter, CancellationToken cancellationToken = default);
    Task<Order?> GetOrderByCondition(FilterDefinition<Order> filter, CancellationToken cancellationToken = default);
    Task<Order?> AddOrder(Order order, CancellationToken cancellationToken = default);
    Task<Order?> UpdateOrder(Order order, CancellationToken cancellationToken = default);
    Task<int> DeleteOrder(Guid orderId, CancellationToken cancellationToken = default);
}