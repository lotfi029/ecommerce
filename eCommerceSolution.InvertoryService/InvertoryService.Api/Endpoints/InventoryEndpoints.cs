using InventoryService.Core.DTOs.Inventories;
using InventoryService.Core.CQRS.Inventories.Commands.Add;
using InventoryService.Core.CQRS.Inventories.Queries.GetAll;
using System.Security.Claims;

namespace InventoryService.Api.Endpoints;

public class InventoryEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/inventories")
            .WithTags(Tags.Inventory)
            .RequireAuthorization();

        group.MapPost("/", Add);
        group.MapGet("/", GetAll);
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
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        var userId = httpContext.User.FindFirst("sub")?.Value!;
        var command = new AddInventoryCommand(userId, request.ProductId, request.Quantity, request.SKU, request.WarehouseId);
        var result = await handler.HandleAsync(command, ct);
        return result.Match(TypedResults.Created, CustomResults.ToProblem);
    }

    private async Task<IResult> GetAll(
        [FromServices] IQueryHandler<GetInventriesQuery, List<InventoryResponse>> handler,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var query = new GetInventriesQuery(userId);

        var result = await handler.HandleAsync(query, ct);
        return result.Match(Results.Ok, CustomResults.ToProblem);

    }
}
