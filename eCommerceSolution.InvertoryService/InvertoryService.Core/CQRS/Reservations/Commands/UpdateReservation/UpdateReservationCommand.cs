namespace InventoryService.Core.CQRS.Reservations.Commands.UpdateReservation;
public record UpdateReservationCommand(
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
            if (await unitOfWork.ReservationRepository.GetByIdAsync(command.Id, ct) is not { } reservation)
                return ReservationErrors.NotFound(command.Id);

            if (reservation.Status == ReservationStatus.Pending)
                return ReservationErrors.InvalidStatus(command.Status.ToString());

            if (command.Status == ReservationStatus.Released)
            {
                reservation.Status = ReservationStatus.Released;
                reservation.UpdatedAt = DateTime.UtcNow;
                await unitOfWork.ReservationRepository.UpdateAsync(reservation, ct);

                return Result.Success();
            }
            else
            {
                var beginTransaction = await unitOfWork.BeginTransactionAsync(ct);

                try
                {
                    if (await unitOfWork.InventoryRepository.FindAsync(ct, reservation.InventoryId) is not { } inventory)
                        return InventoryErrors.NotFound(reservation.InventoryId);

                    var availableQuantity = inventory.Quantity - reservation.Quantity;

                    if (reservation.Quantity > availableQuantity)
                        return InventoryErrors.InsufficientStock(reservation.InventoryId, reservation.Quantity, availableQuantity);
                    inventory.Quantity -= reservation.Quantity;
                    inventory.UpdatedAt = DateTime.UtcNow;
                    await unitOfWork.InventoryRepository.UpdateAsync(inventory, ct);

                    reservation.Status = ReservationStatus.Confirmed;
                    reservation.UpdatedAt = DateTime.UtcNow;
                    await unitOfWork.ReservationRepository.UpdateAsync(reservation, ct);

                    await unitOfWork.CommitTransactionAsync(beginTransaction, ct);
                }
                catch (Exception)
                {
                    await unitOfWork.RollbackTransactionAsync(beginTransaction, ct);
                    throw;
                }
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating reservation {ReservationId}", command.Id);
            return Error.Unexpected(ex.Message);
        }
    }
}
