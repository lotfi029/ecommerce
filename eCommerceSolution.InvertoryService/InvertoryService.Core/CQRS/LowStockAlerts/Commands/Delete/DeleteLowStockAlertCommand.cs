namespace InventoryService.Core.CQRS.LowStockAlerts.Commands.Delete;

public record DeleteLowStockAlertCommand(string UserId, Guid InventoryId) : ICommand;

public class DeleteLowStockAlertCommandHandler(
    IUnitOfWork unitOfWork,
    ILogger<DeleteLowStockAlertCommandHandler> logger) : ICommandHandler<DeleteLowStockAlertCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly ILogger<DeleteLowStockAlertCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result> HandleAsync(DeleteLowStockAlertCommand command, CancellationToken ct = default)
    {
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["InventoryId"] = command.InventoryId,
            ["UserId"] = command.UserId
        });

        _logger.LogInformation(
            LowStockAlertLogEvents.DeletionStarted,
            "Starting deletion of low stock alert.");

        try
        {
            var lowStockAlert = await _unitOfWork.LowStockAlertRepository.DeleteAsync(
                e => e.CreatedBy == command.UserId && 
                     e.InventoryId == command.InventoryId,
                ct);

            if (lowStockAlert == 0)
            {
                _logger.LogWarning(
                    LowStockAlertLogEvents.NotFound,
                    "Low stock alert not found.");
                    
                return LowStockAlertErrors.NotFound(command.InventoryId);
            }

            _logger.LogInformation(
                LowStockAlertLogEvents.DeletionSuccessful,
                "Successfully deleted low stock alert.");

            return Result.Success();
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                LowStockAlertLogEvents.DeletionCancelled,
                "Low stock alert deletion was cancelled.");
                
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                LowStockAlertLogEvents.DeletionFailed,
                ex,
                "Failed to delete low stock alert.");

            return Result.Failure(LowStockAlertErrors.DeletionFailed(command.InventoryId));
        }
    }
}

public static class LowStockAlertLogEvents
{
    public static readonly EventId DeletionStarted = new(100, nameof(DeletionStarted));
    public static readonly EventId DeletionSuccessful = new(101, nameof(DeletionSuccessful));
    public static readonly EventId DeletionFailed = new(102, nameof(DeletionFailed));
    public static readonly EventId DeletionCancelled = new(103, nameof(DeletionCancelled));
    public static readonly EventId NotFound = new(104, nameof(NotFound));
}