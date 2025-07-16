using eCommerce.Core.Entities;
using eCommerce.Core.IRepositoryContracts;
using MongoDB.Driver;

namespace eCommerce.Infrastructure.Repositories;

public class OrderRepository(IMongoDatabase database) : IOrderRepository
{
    private readonly IMongoCollection<Order> _ordersCollection = database.GetCollection<Order>("Orders");

    public async Task<Order?> AddOrder(Order order, CancellationToken cancellationToken = default)
    {
        order.Id = order.Id;
        await _ordersCollection.InsertOneAsync(order, cancellationToken: cancellationToken);
        return order;
    }

    public async Task<IEnumerable<Order>> GetOrders(CancellationToken cancellationToken = default)
    {
        var orders = await _ordersCollection.FindAsync(Builders<Order>.Filter.Empty, cancellationToken: cancellationToken);

        return await orders.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order?>> GetOrdersByCondition(FilterDefinition<Order> filter, CancellationToken cancellationToken = default)
    {
        return await _ordersCollection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrderByCondition(FilterDefinition<Order> filter, CancellationToken cancellationToken = default)
    {
        var result = await _ordersCollection.FindAsync(filter, cancellationToken: cancellationToken);

        return await result.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Order?> UpdateOrder(Order order, CancellationToken cancellationToken = default)
    {
        var result = await _ordersCollection.ReplaceOneAsync(
            o => o.Id == order.Id,
            order,
            cancellationToken: cancellationToken);
        return result.IsAcknowledged && result.ModifiedCount > 0 ? order : null;
    }

    public async Task<int> DeleteOrder(Guid orderId, CancellationToken cancellationToken = default)
    {
        var result = await _ordersCollection.DeleteOneAsync(o => o.Id == orderId, cancellationToken);
        return (int)result.DeletedCount;
    }
}