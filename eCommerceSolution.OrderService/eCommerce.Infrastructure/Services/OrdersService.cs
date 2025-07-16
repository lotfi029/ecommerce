using eCommerce.Core.Abstractions;
using eCommerce.Core.Contracts;
using eCommerce.Core.Entities;
using eCommerce.Core.IRepositoryContracts;
using eCommerce.Core.ServiceContracts;
using eCommerce.Infrastructure.HttpClients;
using Mapster;
using MongoDB.Driver;
using System.Data;

namespace eCommerce.Infrastructure.Services;

public class OrdersService(
    IOrderRepository _orderRepository,
    ProductHttpClient _productHttpClient) : IOrdersService
{
    public async Task<Result<Guid>> AddOrderAsync(Guid userId, CreateOrderRequest request, CancellationToken ct = default)
    {
        try
        {   
            var orderItems = new List<OrderItem>();

            foreach (var item in request.OrderItems)
            {
                var existPrd = await _productHttpClient.GetProductById(item.ProductId, ct);
                if (existPrd.IsFailure)
                    return Error.NotFound("Product.Error", "Product not found");

                orderItems.Add(new()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.Quantity * item.UnitPrice
                });

            }
            var order = new Order
            {
                TotalBill = 0,
                UserId = userId,
                OrderItems = orderItems
            };
            var result = await _orderRepository.AddOrder(order, ct);

            if (result != null)
            {
                return Result.Success(result.Id);
            }

            return Error.InternalServerError("ORDER_CREATE_FAILED", "Failed to create order");
        }
        catch (Exception ex)
        {
            return Error.InternalServerError("ORDER_CREATE_ERROR", $"Error creating order: {ex.Message}");
        }
    }
    public async Task<Result> UpdateOrderRequest(Guid userId, UpdateOrderRequest request, CancellationToken ct = default)
    {
        try
        {
            var order = new Order
            {
                Id = request.Id,
                UserId = userId,
                OrderItems = [.. request.OrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity
                })]
            };

            var result = await _orderRepository.UpdateOrder(order, ct);

            if (result != null)
            {
                return Result.Success();
            }

            return Error.NotFound("OrderNotFound","Order not found or could not be updated");
        }
        catch (Exception ex)
        {
            return Error.NotFound("OrderNotFound",$"Error updating order: {ex.Message}");
        }
    }
    public async Task<Result> DeleteOrderAsync(Guid Id)
    {
        try
        {
            var deletedCount = await _orderRepository.DeleteOrder(Id);

            if (deletedCount > 0)
            {
                return Result.Success();
            }

            return Error.NotFound("OrderNotFound", "Order not found or could not be updated");
        }
        catch (Exception ex)
        {
            return Error.NotFound("OrderNotFound", $"Error updating order: {ex.Message}");
        }
    }
    private async Task<IEnumerable<OrderItemResponse>> GetOrderItemResponse(IEnumerable<OrderItem> orderItems, CancellationToken ct)
    {
        var items = new List<OrderItemResponse>();
        foreach (var item in orderItems)
        {
            var product = await _productHttpClient.GetProductById(item.ProductId, ct);
            if (product.IsFailure)
                continue;
            items.Add(new OrderItemResponse(
                item.ProductId,
                item.UnitPrice,
                item.Quantity,
                item.TotalPrice,
                product.Value.Name,
                product.Value.CategoryName
            ));
        }
        return items ?? [];
    }
    public async Task<Result<IEnumerable<OrderResponse>>> GetOrdersAsync(CancellationToken ct = default)
    {
        try
        {
            var orders = await _orderRepository.GetOrders(ct);
            List<OrderResponse> response = [];
            foreach (var order in orders)
            {
                var items = await GetOrderItemResponse(order.OrderItems, ct);
                if (!items.Any())
                    continue;

                response.Add(
                    new(
                        order.Id,
                        order.UserId,
                        order.CreatedAt,
                        order.TotalBill,
                        [.. items])
                    );
            }
            
            return response ?? [];
        }
        catch (Exception ex)
        {
            return Error.InternalServerError($"Orders.{GetOrdersAsync}", $"An error eccure while retrive data from data base {ex.Message}");
        }
    }
    public async Task<Result<OrderResponse>> GetOrderByConditionAsync(FilterDefinition<Order> filter, CancellationToken ct = default)
    {
        try
        {
            if (await _orderRepository.GetOrderByCondition(filter, ct) is not { } order)
                return Error.NotFound("ORDER_NOT_FOUND", "Order not found");

            var items = await GetOrderItemResponse(order.OrderItems, ct);

            if (!items.Any())
                return Error.NotFound("ORDER_NOT_FOUND", "Order items not found");

            var response = (order, items).Adapt<OrderResponse>();


            return response;
        }
        catch (Exception ex)
        {
            return Error.InternalServerError("ORDER_RETRIEVAL_ERROR", $"Error retrieving order: {ex.Message}");
        }
    }
    public async Task<Result<IEnumerable<OrderResponse>>> GetOrdersByConditionAsync(FilterDefinition<Order> filter, CancellationToken ct = default)
    {
        try
        {
            var orders = await _orderRepository.GetOrdersByCondition(filter, ct);

            if (!orders.Any())
                return Result.Success(Enumerable.Empty<OrderResponse>());

            List<OrderResponse> response = [];
            foreach (var order in orders)
            {
                var items = await GetOrderItemResponse(order!.OrderItems, ct);
                if (!items.Any())
                    continue;

                response.Add(
                    new(
                        order.Id,
                        order.UserId,
                        order.CreatedAt,
                        order.TotalBill,
                        [.. items])
                    );
            }

            return response ?? [];
        }
        catch (Exception ex)
        {
            return Error.InternalServerError("ORDER_RETRIEVAL_ERROR", $"Error retrieving order: {ex.Message}");
        }
    }
}
