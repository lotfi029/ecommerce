namespace InventoryService.Core.CQRS.Reservations.Commands.AddReservation;

public record AddReservationCommand(
    string UserId,
    Guid InventoryId,
    int Quantity
) : ICommand<Guid>;

public class AddReservationCommandHandler(
    IUnitOfWork unitOfWork,
    ILogger<AddReservationCommandHandler> logger) : ICommandHandler<AddReservationCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly ILogger<AddReservationCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result<Guid>> HandleAsync(AddReservationCommand command, CancellationToken ct = default)
    {
        try
        {
            if (await _unitOfWork.InventoryRepository.GetByIdAsync(command.InventoryId, ct) is not { } inventory)
                return InventoryErrors.NotFound(command.InventoryId);
            
            if (command.Quantity <= 0)
                return InventoryErrors.InvalidQuantity(command.Quantity);

            var totalReserved = (await _unitOfWork.ReservationRepository
                .GetAllWithFilters(r => r.InventoryId == command.InventoryId && r.Status != ReservationStatus.Confirmed, ct))
                .Sum(r => r.Quantity);

            var availableQuantity = inventory.Quantity - totalReserved;

            if (command.Quantity > availableQuantity)
                return InventoryErrors.InsufficientStock(command.InventoryId, command.Quantity, availableQuantity);
            
            var reservation = new Reservation
            {
                CreatedBy = command.UserId,
                InventoryId = command.InventoryId,
                Quantity = command.Quantity,
                OrderId = Guid.Empty,
                CreatedAt = DateTime.UtcNow
            };

            using var transaction = await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                var reservationId = await _unitOfWork.ReservationRepository.AddAsync(reservation, ct);
                await _unitOfWork.CommitTransactionAsync(transaction, ct);

                _logger.LogInformation("Created reservation {ReservationId} for inventory {InventoryId}", 
                    reservationId, command.InventoryId);

                return Result.Success(reservationId);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(transaction, ct);
                _logger.LogError(ex, "Failed to create reservation for inventory {InventoryId}", command.InventoryId);
                throw;
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Reservation operation cancelled for inventory {InventoryId}", command.InventoryId);
            throw;
        }
        catch (Exception ex)
        {            
            _logger.LogError(ex, "Error creating reservation for inventory {InventoryId}", command.InventoryId);

            return Error.Unexpected(ex.Message);
        }
    }
}