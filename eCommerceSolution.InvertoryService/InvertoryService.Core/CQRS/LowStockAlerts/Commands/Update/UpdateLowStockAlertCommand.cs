using Microsoft.Extensions.Logging;

namespace InventoryService.Core.CQRS.LowStockAlerts.Commands.Update;

public record UpdateLowStockAlertCommand(string UserId, Guid Inventory, int? Threshold = null, bool? AlertSent = null) : ICommand;

public class UpdateLowStockAlertCommandHandler(
    IUnitOfWork unitOfWork,
    ILogger<UpdateLowStockAlertCommandHandler> logger) : ICommandHandler<UpdateLowStockAlertCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly ILogger<UpdateLowStockAlertCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result> HandleAsync(UpdateLowStockAlertCommand command, CancellationToken ct = default)
    {
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["Inventory"] = command.Inventory,
            ["UserId"] = command.UserId
        });

        _logger.LogInformation(
            LowStockAlertUpdateLogEvents.UpdateStarted,
            "Starting update of low stock alert.");

        try
        {
            var alert = await _unitOfWork.LowStockAlertRepository.GetAsync(
                e => e.InventoryId == command.Inventory && e.CreatedBy == command.UserId, ct);

            if (alert is null)
            {
                _logger.LogWarning(
                    LowStockAlertUpdateLogEvents.NotFound,
                    "Low stock alert not found for update.");
                return LowStockAlertErrors.NotFound(command.Inventory);
            }

            bool changed = false;
            if (command.Threshold.HasValue && alert.Threshold != command.Threshold.Value)
            {
                alert.Threshold = command.Threshold.Value;
                changed = true;
            }
            if (command.AlertSent.HasValue && alert.AlertSent != command.AlertSent.Value)
            {
                alert.AlertSent = command.AlertSent.Value;
                changed = true;
            }

            if (!changed)
            {
                _logger.LogInformation(
                    LowStockAlertUpdateLogEvents.NoChange,
                    "No changes detected for low stock alert.");
                return Result.Success();
            }

            await _unitOfWork.LowStockAlertRepository.UpdateAsync(alert, ct);
            await _unitOfWork.CommitChangesAsync(ct);

            _logger.LogInformation(
                LowStockAlertUpdateLogEvents.UpdateSuccessful,
                "Successfully updated low stock alert.");

            return Result.Success();
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                LowStockAlertUpdateLogEvents.UpdateCancelled,
                "Low stock alert update was cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                LowStockAlertUpdateLogEvents.UpdateFailed,
                ex,
                "Failed to update low stock alert.");
            return Result.Failure(LowStockAlertErrors.CreationFailed(command.Inventory));
        }
    }
}

public static class LowStockAlertUpdateLogEvents
{
    public static readonly EventId UpdateStarted = new(300, nameof(UpdateStarted));
    public static readonly EventId UpdateSuccessful = new(301, nameof(UpdateSuccessful));
    public static readonly EventId UpdateFailed = new(302, nameof(UpdateFailed));
    public static readonly EventId UpdateCancelled = new(303, nameof(UpdateCancelled));
    public static readonly EventId NotFound = new(304, nameof(NotFound));
    public static readonly EventId NoChange = new(305, nameof(NoChange));
}
