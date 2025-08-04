namespace InventoryService.Core.CQRS.LowStockAlerts.Commands.Add;

public record AddLowStockAlertCommand(string UserId, Guid InventoryId, int Threshold) : ICommand<Guid>;

public class AddLowStockAlertCommandHandler(IUnitOfWork unitOfWork, ILogger<AddLowStockAlertCommandHandler> logger) : ICommandHandler<AddLowStockAlertCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly ILogger<AddLowStockAlertCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result<Guid>> HandleAsync(AddLowStockAlertCommand command, CancellationToken ct = default)
    {
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["InventoryId"] = command.InventoryId,
            ["Threshold"] = command.Threshold,
            ["UserId"] = command.UserId
        });

        _logger.LogInformation(
            LowStockAlertAddLogEvents.AddStarted,
            "Starting to add low stock alert.");

        try
        {
            if (!await _unitOfWork.InventoryRepository.ExistsAsync(e => e.CreatedBy == command.UserId && e.Id == command.InventoryId, ct))
            {
                _logger.LogWarning(
                    LowStockAlertAddLogEvents.InventoryNotFound,
                    "Inventory not found for inventory: {inventory}, UserId: {UserId}",
                    command.InventoryId, command.UserId);
                return LowStockAlertErrors.NotFound(command.InventoryId);
            }

            if (await _unitOfWork.LowStockAlertRepository.ExistsAsync(e => e.InventoryId == command.InventoryId, ct))
            {
                _logger.LogWarning(
                    LowStockAlertAddLogEvents.AlreadyExists,
                    "Low stock alert already exists for InventoryId: {InventoryId}",
                    command.InventoryId);
                return LowStockAlertErrors.AlreadyExists(command.InventoryId);
            }

            var lowStockAlert = new LowStockAlert
            {
                InventoryId = command.InventoryId,
                Threshold = command.Threshold,
                AlertSent = false
            };

            var alertId = await _unitOfWork.LowStockAlertRepository.AddAsync(lowStockAlert, ct);

            if (alertId == Guid.Empty)
            {
                _logger.LogError(
                    LowStockAlertAddLogEvents.CreationFailed,
                    "Failed to create low stock alert for InventoryId: {InventoryId}",
                    command.InventoryId);
                return LowStockAlertErrors.CreationFailed(command.InventoryId);
            }

            _logger.LogInformation(
                LowStockAlertAddLogEvents.AddSuccessful,
                "Successfully added low stock alert. InventoryId: {InventoryId}, AlertId: {AlertId}",
                command.InventoryId, alertId);

            return alertId;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                LowStockAlertAddLogEvents.AddCancelled,
                "Low stock alert add operation was cancelled. InventoryId: {InventoryId}, UserId: {UserId}",
                command.InventoryId, command.UserId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                LowStockAlertAddLogEvents.AddFailed,
                ex,
                "Error adding low stock alert. InventoryId: {InventoryId}, UserId: {UserId}",
                command.InventoryId, command.UserId);
            return LowStockAlertErrors.CreationFailed(command.InventoryId);
        }
    }
}

public static class LowStockAlertAddLogEvents
{
    public static readonly EventId AddStarted = new(200, nameof(AddStarted));
    public static readonly EventId AddSuccessful = new(201, nameof(AddSuccessful));
    public static readonly EventId AddFailed = new(202, nameof(AddFailed));
    public static readonly EventId AddCancelled = new(203, nameof(AddCancelled));
    public static readonly EventId InventoryNotFound = new(204, nameof(InventoryNotFound));
    public static readonly EventId AlreadyExists = new(205, nameof(AlreadyExists));
    public static readonly EventId CreationFailed = new(206, nameof(CreationFailed));
}