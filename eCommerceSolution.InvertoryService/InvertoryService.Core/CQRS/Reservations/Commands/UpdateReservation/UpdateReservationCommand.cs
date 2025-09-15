namespace InventoryService.Core.CQRS.Reservations.Commands.UpdateReservation;
public record UpdateReservationCommand(
    string UserId,
    Guid Id, 
    ReservationStatus Status) : ICommand;

public class UpdateReservationCommandHandler(
    IUnitOfWork unitOfWork,
    ILogger<UpdateReservationCommandHandler> logger) : ICommandHandler<UpdateReservationCommand>
{
    public async Task<Result> HandleAsync(UpdateReservationCommand command, CancellationToken ct = default) 
    {
        try
        {
            if (await unitOfWork.ReservationRepository.GetAsync(e => e.Id == command.Id && e.CreatedBy == command.UserId, ct) is not { } reservation)
                return ReservationErrors.NotFound(command.Id);

            if (reservation.Status is not ReservationStatus.Pending)
                return ReservationErrors.InvalidStatus(command.Status.ToString());


            return command.Status switch
            {
                ReservationStatus.Released => await HandleReleaseAsync(reservation, ct),
                ReservationStatus.Confirmed => await HandleConfirmAsync(reservation, ct),
                _ => ReservationErrors.InvalidStatus(command.Status.ToString())
            };
           
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating reservation {ReservationId}", command.Id);
            return Error.Unexpected(ex.Message);
        }
    }

    private async Task<Result> HandleReleaseAsync(Reservation reservation, CancellationToken ct)
    {
        try
        {
            reservation.Status = ReservationStatus.Released;

            await unitOfWork.ReservationRepository.UpdateAsync(reservation, ct);
            await unitOfWork.CommitChangesAsync(ct);
            return Result.Success();
        }
        catch
        {
            throw;
        }
    }
    private async Task<Result> HandleConfirmAsync(Reservation reservation, CancellationToken ct)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(ct);

        try
        {
            if (await unitOfWork.InventoryRepository.FindAsync(ct, reservation.InventoryId) is not { } inventory)
                return InventoryErrors.NotFound(reservation.InventoryId);

            if (reservation.Quantity > inventory.Quantity)
                return InventoryErrors.InsufficientStock(reservation.InventoryId, reservation.Quantity, inventory.Quantity);

            inventory.Quantity -= reservation.Quantity;
            await unitOfWork.InventoryRepository.UpdateAsync(inventory, ct);

            reservation.Status = ReservationStatus.Confirmed;
            await unitOfWork.ReservationRepository.UpdateAsync(reservation, ct);

            await unitOfWork.CommitTransactionAsync(transaction, ct);

            return Result.Success();
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(transaction, ct);
            throw;
        }
    }
}
