using InventoryService.Core.DTOs.Inventories;
using InventoryService.Core.CQRS.Inventories.Commands.Add;
using InventoryService.Core.CQRS.Inventories.Queries.GetAll;
using InventoryService.Core.CQRS.Inventories.Commands.Update;
using InventoryService.Core.CQRS.Inventories.Queries.Get;

namespace InventoryService.Api.Endpoints;

public class InventoryEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/inventories")
            .WithTags(Tags.Inventory)
            .RequireAuthorization();

        group.MapPost("/", Add);
        group.MapPut("/{id:guid}/update-reorderlevel", Update);
        group.MapGet("/{id:guid}", GetById);
        group.MapGet("/", GetAll);
        group.MapGet("/{productId:guid}/by-product", GetByProductId);
        group.MapGet("/{sku}/by-sku", GetBySKU);
        group.MapGet("/low-stock", GetLowStockInventories);
        group.MapGet("/by-warehouse/{warehouseId:guid}", GetByWarehouse);
    }

    public async Task<IResult> Add(
        [FromBody] InventoryRequest request,
        [FromServices] ICommandHandler<AddInventoryCommand, Guid> handler,
        [FromServices] IValidator<InventoryRequest> validator,
        HttpContext httpContext,
        CancellationToken ct
    )
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());
        
        var userId = httpContext.GetUserId();
        var command = new AddInventoryCommand(userId, request.ProductId, request.Quantity, request.SKU, request.WarehouseId);
        var result = await handler.HandleAsync(command, ct);
        return result.Match(TypedResults.Created, CustomResults.ToProblem);
    }
    private async Task<IResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateReorderLevelRequest request,
        [FromServices] ICommandHandler<UpdateReorderLevelCommand> handler,
        [FromServices] IValidator<UpdateReorderLevelRequest> validator,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());
        
        var userId = httpContext.GetUserId();
        var command = new UpdateReorderLevelCommand(userId, id, request.ReorderLevel);
        var result = await handler.HandleAsync(command, ct);

        return result.Match(TypedResults.NoContent, CustomResults.ToProblem);
    }
    private async Task<IResult> GetByProductId(
        [FromRoute] Guid productId,
        [FromServices] IQueryHandler<GetInventoriesByFiltersQuery, IEnumerable<InventoryResponse>> handler,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        var userId = httpContext.GetUserId();
        var query = new GetInventoriesByFiltersQuery(
            UserId: userId,
            WarehouseId: null,
            ProductId: productId, 
            SKU: null,
            ReorderLevel: null);
        var result = await handler.HandleAsync(query, ct);
        return result.Match(Results.Ok, CustomResults.ToProblem);
    }
    private async Task<IResult> GetBySKU(
        [FromRoute] string sku,
        [FromServices] IQueryHandler<GetInventoriesByFiltersQuery, IEnumerable<InventoryResponse>> handler,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        var userId = httpContext.GetUserId();

        var query = new GetInventoriesByFiltersQuery(
            UserId: userId,
            WarehouseId: null,
            ProductId: null, 
            SKU: sku,
            ReorderLevel: null);

        var result = await handler.HandleAsync(query, ct);
        return result.Match(Results.Ok, CustomResults.ToProblem);
    }
    private async Task<IResult> GetLowStockInventories(
        [FromServices] IQueryHandler<GetInventoriesByFiltersQuery, IEnumerable<InventoryResponse>> handler,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        var userId = httpContext.GetUserId();
        var query = new GetInventoriesByFiltersQuery(
            UserId: userId,
            WarehouseId: null,
            ProductId: null, 
            SKU: null,
            ReorderLevel: null,
            IsReorderLevel: true);

        var result = await handler.HandleAsync(query, ct);
        return result.Match(Results.Ok, CustomResults.ToProblem);
    }
    private async Task<IResult> GetByWarehouse(
        [FromRoute] Guid warehouseId,
        [FromServices] IQueryHandler<GetInventoriesByFiltersQuery, IEnumerable<InventoryResponse>> handler,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        var userId = httpContext.GetUserId();
        var query = new GetInventoriesByFiltersQuery(
            UserId: userId,
            WarehouseId: warehouseId,
            ProductId: null, 
            SKU: null,
            ReorderLevel: null);

        var result = await handler.HandleAsync(query, ct);
        return result.Match(Results.Ok, CustomResults.ToProblem);
    }
    private async Task<IResult> GetById(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetInventoryByIdQuery, InventoryResponse> handler,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        var userId = httpContext.GetUserId();
        var query = new GetInventoryByIdQuery(userId, id);
        var result = await handler.HandleAsync(query, ct);

        return result.Match(Results.Ok, CustomResults.ToProblem);
    }
    private async Task<IResult> GetAll(
        [FromServices] IQueryHandler<GetInventriesQuery, List<InventoryResponse>> handler,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        var userId = httpContext.GetUserId();
        var query = new GetInventriesQuery(userId);

        var result = await handler.HandleAsync(query, ct);
        return result.Match(Results.Ok, CustomResults.ToProblem);
    }
}
