using InventoryService.Core.DTOs.Inventories;
using InventoryService.Core.CQRS.Inventories.Commands.Add;

namespace InventoryService.Api.Endpoints;

public class InventoryEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/inventories")
            .WithTags(Tags.Inventory)
            .RequireAuthorization();

        group.MapPost("/", AddInventory);
    }

    public async Task<IResult> AddInventory(
        [FromBody] InventoryRequest request,
        [FromServices] ICommandHandler<AddInventoryCommand, Guid> handler,
        [FromServices] IValidator<InventoryRequest> validator,
        HttpContext httpContext,
        CancellationToken ct
    )
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        var userId = httpContext.User.FindFirst("sub")?.Value!;
        var command = new AddInventoryCommand(userId, request.ProductId, request.Quantity, request.SKU, request.WarehouseId);
        var result = await handler.HandleAsync(command, ct);
        return result.Match(TypedResults.Created, CustomResults.ToProblem);
    }
}
