using eCommerce.SharedKernal.Messaging;
using FluentValidation;
using InventoryService.Api.Extensions;
using InventoryService.Api.Infrastracture;
using InventoryService.Core.CQRS.Warehouses.Commands.Add;
using InventoryService.Core.CQRS.Warehouses.Commands.Delete;
using InventoryService.Core.CQRS.Warehouses.Commands.Update;
using InventoryService.Core.CQRS.Warehouses.Queries.Get;
using InventoryService.Core.CQRS.Warehouses.Queries.GetAll;
using InventoryService.Core.DTOs.Warehouses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryService.Api.Endpoints;

public class WarehouseEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/warehouses")
            .WithTags(Tags.Warehouse)
            .RequireAuthorization();

        group.MapPost("/", AddWarehouse);
        group.MapPut("/{id:guid}", UpdateWarehouse);
        group.MapDelete("/{id:guid}", DeleteWarehouse);

        group.MapGet("/", GetWarehouses);
        group.MapGet("/{id:guid}", GetWarehouse);
    }

    private async Task<IResult> AddWarehouse(
        [FromBody] WarehouseRequest request,
        IValidator<WarehouseRequest> validator,
        ICommandHandler<AddWarehouseCommand, Guid> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false} validationResult)
        {
            var error = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage })
                .ToList();

            return Results.BadRequest(error);
        }
        var command = new AddWarehouseCommand(request);
        var result = await handler.HandleAsync(command, ct);

        return result.Match(Results.Created, CustomResults.ToProblem);
    }
    private async Task<IResult> UpdateWarehouse(
        [FromRoute] Guid id,
        [FromBody] WarehouseRequest request,
        IValidator<WarehouseRequest> validator,
        ICommandHandler<UpdateWarehouseCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false} validationResult)
        {
            var error = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage })
                .ToList();

            return Results.BadRequest(error);
        }
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var command = new UpdateWarehouseCommand(userId, id, request);
        var result = await handler.HandleAsync(command, ct);

        return result.Match(Results.NoContent, CustomResults.ToProblem);
    }
    private async Task<IResult> DeleteWarehouse(
        [FromRoute] Guid id,
        ICommandHandler<DeleteWarehouseCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new DeleteWarehouseCommand(userId, id);
        var result = await handler.HandleAsync(command, ct);
        return result.Match(Results.NoContent, CustomResults.ToProblem);
    }
    private async Task<IResult> GetWarehouse(
        [FromRoute] Guid id,
        IQueryHandler<GetWarehouseByIdQuery, WarehouseResponse> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var query = new GetWarehouseByIdQuery(userId,id);
        var result = await handler.HandleAsync(query, ct);
        return result.Match(Results.Ok, CustomResults.ToProblem);
    }
    private async Task<IResult> GetWarehouses(
        IQueryHandler<GetAllWarehouseQuery, IEnumerable<WarehouseResponse>> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var query = new GetAllWarehouseQuery(userId);
        var result = await handler.HandleAsync(query, ct);
        return result.Match(Results.Ok, CustomResults.ToProblem);
    }
}
