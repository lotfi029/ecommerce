using eCommerce.API.Extensions;
using eCommerce.API.Infrastracture;
using eCommerce.Core.Abstractions;
using eCommerce.Core.Contracts;
using eCommerce.Core.Entities;
using eCommerce.Core.ServiceContracts;
using FluentValidation;
using MongoDB.Driver;

namespace eCommerce.API.Endpoints;

public class OrderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders")
            .WithTags(Tags.Order)
            .RequireAuthorization();

        group.MapPost("/", AddOrderAsync);
        group.MapGet("/{id:guid}", GetOrderByIdAsync);
        group.MapGet("search-productId/{productId:guid}", GetOrderByProductId);
        group.MapGet("search-date/{dateTime:datetime}", GetOrderByDate);

        group.MapGet("/", GetAllOrdersAsync);
        group.MapGet("/user", GetOrdersByUserIdAsync);
        group.MapPut("/{id:guid}", UpdateOrderAsync);
        group.MapDelete("/{id:guid}", DeleteOrderAsync);
    }

    private static async Task<IResult> AddOrderAsync(
        CreateOrderRequest request,
        IOrdersService service,
        IValidator<CreateOrderRequest> validator,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage });
            return Results.BadRequest(errors);
        }

        var userId = context.GetUserId();
        
        var result = await service.AddOrderAsync(userId, request, cancellationToken);
        
        return result.Match(Results.Created, CustomResults.ToProblem);
    }
    private static async Task<IResult> GetOrderByIdAsync(
        Guid id,
        IOrdersService service,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var userId = httpContext.GetUserId();
        var filter = Builders<Order>.Filter.Eq(o => o.Id, id);
        var result = await service.GetOrderByConditionAsync(filter, cancellationToken);
        
        return result.Match(Results.Ok, CustomResults.ToProblem);
    }
    private static async Task<IResult> GetOrderByProductId(
        Guid productId,
        IOrdersService ordersService,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.GetUserId();
        var filter = Builders<Order>.Filter.ElemMatch(
            t => t.OrderItems,
            Builders<OrderItem>.Filter.Eq(oi => oi.ProductId, productId)
            );

        var result = await ordersService.GetOrdersByConditionAsync(filter, ct);
        return result.Match(Results.Ok, CustomResults.ToProblem);

    }
    private static async Task<IResult> GetOrderByDate(
        DateTime dateTime,
        IOrdersService ordersService,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.GetUserId();
        var filter = Builders<Order>.Filter.Eq(
            t => t.CreatedAt.ToString("yyyy-MM-dd"),
            dateTime.ToString("yyyy-MM-dd"));

        var result = await ordersService.GetOrdersByConditionAsync(filter, ct);
        return result.Match(Results.Ok, CustomResults.ToProblem);

    }
    private static async Task<IResult> GetAllOrdersAsync(
        IOrdersService service,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var userId = httpContext.GetUserId();
        var result = await service.GetOrdersAsync(cancellationToken);

        return result.Match(Results.Ok, CustomResults.ToProblem);

    }

    private static async Task<IResult> GetOrdersByUserIdAsync(
        IOrdersService service,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var userId = httpContext.GetUserId();

        var filter = Builders<Order>.Filter.Eq(o => o.UserId, userId);
        var result = await service.GetOrdersByConditionAsync(filter, cancellationToken);

        return result.Match(Results.Ok, CustomResults.ToProblem);
    }

    private static async Task<IResult> UpdateOrderAsync(
        Guid id,
        UpdateOrderRequest request,
        IOrdersService service,
        IValidator<UpdateOrderRequest> validator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var userId = httpContext.GetUserId();
        if (id != request.Id)
        {
            return Results.BadRequest("Order ID in URL does not match the ID in the request body.");
        }

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage });
            return Results.BadRequest(errors);
        }

        var result = await service.UpdateOrderRequest(userId, request, cancellationToken);

        return result.Match(Results.NoContent, CustomResults.ToProblem);

    }

    private static async Task<IResult> DeleteOrderAsync(
        Guid id,
        IOrdersService service,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var userId = httpContext.GetUserId();
        var result = await service.DeleteOrderAsync(id);
        return result.Match(Results.NoContent, CustomResults.ToProblem);

    }
}
