using InventoryService.Core.CQRS.Reservations.Commands.AddReservation;
using InventoryService.Core.CQRS.Reservations.Commands.UpdateReservation;
using InventoryService.Core.CQRS.Reservations.Queries.Get;
using InventoryService.Core.CQRS.Reservations.Queries.GetAll;
using InventoryService.Core.DTOs;
using InventoryService.Core.DTOs.Reservations;
using InventoryService.Core.Enums;

namespace InventoryService.Api.Endpoints;

public class ReservationEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/reservations")
            .WithTags(Tags.Reservation)
            .RequireAuthorization();

        group.MapPost("/{inventoryId:guid}/", Add);
        group.MapPut("/{id:guid}/released", Released);
        group.MapPut("/{id:guid}/confirm", Confirm);

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);
        group.MapGet("/by-inventory/{inventoryId:guid}", GetByInventoryId);
    }

    private async Task<IResult> Add(
        [FromRoute] Guid inventoryId,
        [FromBody] ReservationRequest request,
        ICommandHandler<AddReservationCommand, Guid> handler,
        IValidator<ReservationRequest> validator,
        HttpContext context,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var userId = context.GetUserId();
        var command = new AddReservationCommand(userId, inventoryId, request.Quantity);
        var result = await handler.HandleAsync(command, ct);

        return result.Match(
            reservationId => Results.Created($"/api/reservations/{reservationId}", reservationId),
            CustomResults.ToProblem
        );
    }
    private async Task<IResult> Released(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<UpdateReservationCommand> handler,
        HttpContext context,
        CancellationToken ct)
    {
        var userId = context.GetUserId();
        var command = new UpdateReservationCommand(userId, id, ReservationStatus.Released);
        var result = await handler.HandleAsync(command, ct);

        return result.Match(Results.NoContent, CustomResults.ToProblem);
    }

    private async Task<IResult> Confirm(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<UpdateReservationCommand> handler,
        HttpContext context,
        CancellationToken ct)
    {
        var userId = context.GetUserId();
        var command = new UpdateReservationCommand(userId, id, ReservationStatus.Confirmed);
        var result = await handler.HandleAsync(command, ct);
        return result.Match(Results.NoContent, CustomResults.ToProblem);
    }

    private async Task<IResult> GetAll(
        [FromServices] IQueryHandler<GetReservationWithFiltersQuery, IEnumerable<ReservationResponse>> handler,
        HttpContext context,
        CancellationToken ct)
    {
        var userId = context.GetUserId();
        var query = new GetReservationWithFiltersQuery(userId, InventoryId: null, OrderId: null);
        var result = await handler.HandleAsync(query, ct);

        return result.Match(Results.Ok, CustomResults.ToProblem);
    }

    private async Task<IResult> GetById(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetReservationByIdQuery, ReservationResponse> handler,
        HttpContext context,
        CancellationToken ct)
    {
        var userId = context.GetUserId();
        var query = new GetReservationByIdQuery(userId, id);
        var result = await handler.HandleAsync(query, ct);
        return result.Match(Results.Ok, CustomResults.ToProblem);
    }

    private async Task<IResult> GetByInventoryId(
        [FromRoute] Guid inventoryId,
        [FromServices] IQueryHandler<GetReservationWithFiltersQuery, IEnumerable<ReservationResponse>> handler,
        HttpContext context,
        CancellationToken ct)
    {
        var userId = context.GetUserId();
        var query = new GetReservationWithFiltersQuery(userId, InventoryId: inventoryId, OrderId: null);
        var result = await handler.HandleAsync(query, ct);
        return result.Match(Results.Ok, CustomResults.ToProblem);
    }
}
