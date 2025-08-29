
using InventoryService.Core.Enums;

namespace InventoryService.Core.CQRS.Reservations.Commands.UpdateReservation;
public record UpdateReservationCommand(Guid Id, ReservationStatus Status) : ICommand;

public class UpdateReservationCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateReservationCommand>
{
    public async Task<Result> HandleAsync(UpdateReservationCommand command, CancellationToken ct = default) 
    {
        // canceled - release the stock

        if (await unitOfWork.ReservationRepository.GetByIdAsync(command.Id, ct) is not { } reservation)
            return ReservationErrors.NotFound(command.Id);

        if (reservation.Status == ReservationStatus.Reserved)
        {
            return ReservationErrors.InvalidStatus(command.Status.ToString());
        }
        

        if (command.Status == ReservationStatus.Cancelled)
        {
            reservation.Status = ReservationStatus.Cancelled;
            reservation.UpdatedAt = DateTime.UtcNow;
            await unitOfWork.ReservationRepository.UpdateAsync(reservation, ct);
            await unitOfWork.CommitChangesAsync(ct);
            return Result.Success();
        }

        if (await unitOfWork.InventoryRepository.FindAsync(ct, [reservation.InventoryId]) is not { } inventory)
            return InventoryErrors.NotFound(reservation.InventoryId);



        // completed 

        throw new NotImplementedException();
    }
}
