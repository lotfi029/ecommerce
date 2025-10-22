using InventoryService.Core.CQRS.Transactions.Commands.Add;
using InventoryService.Core.CQRS.Transactions.Queries.Get;
using InventoryService.Core.CQRS.Transactions.Queries.GetAll;
using InventoryService.Core.DTOs.Transactions;
using InventoryService.Core.Enums;

namespace InventoryService.Api.Endpoints;

public class TransactionEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/transactions")
            .WithTags(Tags.Inventory)
            .RequireAuthorization();

        group.MapPost("/{inventoryId:guid}/in", AddIN);
        group.MapPost("/{inventoryId:guid}/out", AddOUT);
        group.MapPost("/{inventoryId:guid}/return", AddRETURN);
        group.MapGet("/{id:guid}", GetTransactionById);
        group.MapGet("/", GetTransactions);
        group.MapGet("/by-inventory/{inventoryId:guid}", GetTransactionsByInventoryId);
        group.MapGet("/by-time-range", GetTransactionsByTimeRange);
    }


    private static async Task<IResult> AddIN(
        [FromRoute] Guid inventoryId,
        [FromBody] TransactionRequest request,
        [FromServices] ICommandHandler<AddTransactionCommand, Guid> handler,
        IValidator<TransactionRequest> validator,
        HttpContext httpContext,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false} validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var userId = httpContext.GetUserId();
        var command = new AddTransactionCommand(
            userId, 
            inventoryId, 
            InventoryChangeType.IN, 
            request
        );
        var result = await handler.HandleAsync(command, ct);
        return result.Match(id => Results.Created($"/api/transactions/{id}", new { Id = id }), CustomResults.ToProblem);
    }
    private static async Task<IResult> AddOUT(
        [FromRoute] Guid inventoryId,
        [FromBody] TransactionRequest request,
        [FromServices] ICommandHandler<AddTransactionCommand, Guid> handler,
        IValidator<TransactionRequest> validator,
        HttpContext httpContext,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false} validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var userId = httpContext.GetUserId();
        var command = new AddTransactionCommand(
            userId, 
            inventoryId, 
            InventoryChangeType.OUT, 
            request
        );
        var result = await handler.HandleAsync(command, ct);
        return result.Match(id => Results.Created($"/api/transactions/{id}", new { Id = id }), CustomResults.ToProblem);
    }
    private static async Task<IResult> AddRETURN(
        [FromRoute] Guid inventoryId,
        [FromBody] TransactionRequest request,
        [FromServices] ICommandHandler<AddTransactionCommand, Guid> handler,
        IValidator<TransactionRequest> validator,
        HttpContext httpContext,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false} validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var userId = httpContext.GetUserId();
        var command = new AddTransactionCommand(
            userId, 
            inventoryId, 
            InventoryChangeType.RETURN, 
            request
        );
        var result = await handler.HandleAsync(command, ct);
        return result.Match(id => Results.Created($"/api/transactions/{id}", new { Id = id }), CustomResults.ToProblem);
    }
    private static async Task<IResult> GetTransactionById(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetTransactionByIdQuery, TransactionResponse> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.GetUserId();
        var query = new GetTransactionByIdQuery(userId, id);
        var result = await handler.HandleAsync(query, ct);
        return result.Match(x => TypedResults.Ok(x), CustomResults.ToProblem);
    }
    private static async Task<IResult> GetTransactions(
        [FromServices] IQueryHandler<GetTransactionsQuery, IEnumerable<TransactionResponse>> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.GetUserId();
        
        var query = new GetTransactionsQuery(
            UserId: userId,
            InventoryId: null,
            ChangeType: null,
            From: null,
            To: null
            );

        var result = await handler.HandleAsync(query, ct);
        return result.Match(x => TypedResults.Ok(x), CustomResults.ToProblem);
    }
    private async Task<IResult> GetTransactionsByInventoryId(
        [FromRoute] Guid inventoryId,
        [FromServices] IQueryHandler<GetTransactionsQuery, IEnumerable<TransactionResponse>> handler,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        var userId = httpContext.GetUserId();
        var query = new GetTransactionsQuery(
            UserId: userId,
            InventoryId: inventoryId,
            ChangeType: null,
            From: null,
            To: null
            );

        var result = await handler.HandleAsync(query, ct);
        return result.Match(x => TypedResults.Ok(x), CustomResults.ToProblem);
    }
    private async Task<IResult> GetTransactionsByTimeRange(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromServices] IQueryHandler<GetTransactionsQuery, IEnumerable<TransactionResponse>> handler,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        var userId = httpContext.GetUserId();
        var query = new GetTransactionsQuery(
            UserId: userId,
            InventoryId: null,
            ChangeType: null,
            From: from,
            To: to
            );
        var result = await handler.HandleAsync(query, ct);
        return result.Match(x => TypedResults.Ok(x), CustomResults.ToProblem);
    }

}
