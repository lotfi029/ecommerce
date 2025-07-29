using Microsoft.Extensions.Logging;

namespace InventoryService.Core.CQRS.LowStockAlerts.Commands.Add;

public record AddLowStockAlertCommand(string UserId, Guid ProductId, string SKU) : ICommand<Guid>;

public class AddLowStockAlertCommandHandler(IUnitOfWork unitOfWork, ILogger<AddLowStockAlertCommandHandler> logger) : ICommandHandler<AddLowStockAlertCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly ILogger<AddLowStockAlertCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result<Guid>> HandleAsync(AddLowStockAlertCommand command, CancellationToken ct = default)
    {
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["ProductId"] = command.ProductId,
            ["SKU"] = command.SKU,
            ["UserId"] = command.UserId
        });

        _logger.LogInformation(
            LowStockAlertAddLogEvents.AddStarted,
            "Starting to add low stock alert. ProductId: {ProductId}, SKU: {SKU}, UserId: {UserId}",
            command.ProductId, command.SKU, command.UserId);

        try
        {
            if (!await _unitOfWork.InventoryRepository.ExistsAsync(e => e.CreatedBy == command.UserId && e.SKU == command.SKU && e.ProductId == command.ProductId, ct))
            {
                _logger.LogWarning(
                    LowStockAlertAddLogEvents.InventoryNotFound,
                    "Inventory not found for ProductId: {ProductId}, SKU: {SKU}, UserId: {UserId}",
                    command.ProductId, command.SKU, command.UserId);
                return InventoryErrors.NotFound(command.ProductId);
            }

            if (await _unitOfWork.LowStockAlertRepository.ExistsAsync(e => e.SKU == command.SKU && e.ProductId == command.ProductId, ct))
            {
                _logger.LogWarning(
                    LowStockAlertAddLogEvents.AlreadyExists,
                    "Low stock alert already exists for ProductId: {ProductId}, SKU: {SKU}",
                    command.ProductId, command.SKU);
                return LowStockAlertErrors.AlreadyExists(command.ProductId, command.SKU);
            }

            var lowStockAlert = new LowStockAlert
            {
                ProductId = command.ProductId,
                SKU = command.SKU,
                Threshold = 10,
                AlertSent = false
            };

            var alertId = await _unitOfWork.LowStockAlertRepository.AddAsync(lowStockAlert, ct);

            if (alertId == Guid.Empty)
            {
                _logger.LogError(
                    LowStockAlertAddLogEvents.CreationFailed,
                    "Failed to create low stock alert for ProductId: {ProductId}, SKU: {SKU}",
                    command.ProductId, command.SKU);
                return LowStockAlertErrors.CreationFailed(command.ProductId, command.SKU);
            }

            _logger.LogInformation(
                LowStockAlertAddLogEvents.AddSuccessful,
                "Successfully added low stock alert. ProductId: {ProductId}, SKU: {SKU}, AlertId: {AlertId}",
                command.ProductId, command.SKU, alertId);

            return alertId;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                LowStockAlertAddLogEvents.AddCancelled,
                "Low stock alert add operation was cancelled. ProductId: {ProductId}, SKU: {SKU}, UserId: {UserId}",
                command.ProductId, command.SKU, command.UserId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                LowStockAlertAddLogEvents.AddFailed,
                ex,
                "Error adding low stock alert. ProductId: {ProductId}, SKU: {SKU}, UserId: {UserId}",
                command.ProductId, command.SKU, command.UserId);
            return LowStockAlertErrors.CreationFailed(command.ProductId, command.SKU);
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