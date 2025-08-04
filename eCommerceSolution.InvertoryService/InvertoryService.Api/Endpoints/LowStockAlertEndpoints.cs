using InventoryService.Core.CQRS.LowStockAlerts.Commands.Add;
using InventoryService.Core.CQRS.LowStockAlerts.Commands.Delete;
using InventoryService.Core.CQRS.LowStockAlerts.Commands.Update;
using InventoryService.Core.CQRS.LowStockAlerts.Queries.Get;
using InventoryService.Core.CQRS.LowStockAlerts.Queries.GetAll;
using InventoryService.Core.DTOs.LowStockAlerts;
using System.Security.Claims;

namespace InventoryService.Api.Endpoints;

public class LowStockAlertEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/low-stock-alerts")
            .WithTags(Tags.LowStockAlert)
            .RequireAuthorization();

        group.MapPost("/", AddLowStockAlert);
        group.MapPut("/{id:guid}", UpdateLowStockAlert);
        group.MapDelete("/{inventoryId:guid}", DeleteLowStockAlert);

        group.MapGet("/", GetLowStockAlerts);
        group.MapGet("/{inventoryId:guid}", GetLowStockAlert);
    }

    private async Task<IResult> AddLowStockAlert(
        [FromBody] LowStockAlertRequest request,
        [FromServices] ICommandHandler<AddLowStockAlertCommand, Guid> handler,
        [FromServices] IValidator<LowStockAlertRequest> validator,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var command = new AddLowStockAlertCommand(userId, request.InventoryId, request.Threshold);
        var result = await handler.HandleAsync(command, ct);
        return result.Match(TypedResults.Created, CustomResults.ToProblem);
    }

    private async Task<IResult> UpdateLowStockAlert(
        [FromRoute] Guid id,
        [FromBody] LowStockAlertRequest request,
        [FromServices] ICommandHandler<UpdateLowStockAlertCommand> handler,
        [FromServices] IValidator<LowStockAlertRequest> validator,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var command = new UpdateLowStockAlertCommand(userId, request.InventoryId, request.Threshold, false);
        var result = await handler.HandleAsync(command, ct);
        return result.Match(TypedResults.NoContent, CustomResults.ToProblem);
    }

    private async Task<IResult> DeleteLowStockAlert(
        [FromRoute] Guid inventoryId,
        [FromQuery] string sku,
        [FromServices] ICommandHandler<DeleteLowStockAlertCommand> handler,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var command = new DeleteLowStockAlertCommand(userId, inventoryId);
        var result = await handler.HandleAsync(command, ct);
        return result.Match(TypedResults.NoContent, CustomResults.ToProblem);
    }

    private async Task<IResult> GetLowStockAlerts(
        [FromServices] IQueryHandler<GetAllLowStockAlertQuery, IEnumerable<LowStockAlertResponse>> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var query = new GetAllLowStockAlertQuery(userId);
        var result = await handler.HandleAsync(query, ct);
        return result.Match(Results.Ok, CustomResults.ToProblem);
    }

    private async Task<IResult> GetLowStockAlert(
        [FromRoute] Guid inventoryId,
        [FromQuery] string sku,
        [FromServices] IQueryHandler<GetLowStockAlertQuery, LowStockAlertResponse> handler,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var query = new GetLowStockAlertQuery(userId, inventoryId);
        var result = await handler.HandleAsync(query, ct);

        return result.Match(Results.Ok, CustomResults.ToProblem);
    }
}
