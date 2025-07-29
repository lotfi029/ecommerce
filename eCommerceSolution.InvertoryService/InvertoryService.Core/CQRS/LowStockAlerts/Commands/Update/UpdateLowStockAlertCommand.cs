using Microsoft.Extensions.Logging;

namespace InventoryService.Core.CQRS.LowStockAlerts.Commands.Update;

public record UpdateLowStockAlertCommand(string UserId, Guid ProductId, string SKU, int? Threshold = null, bool? AlertSent = null) : ICommand;

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
            ["ProductId"] = command.ProductId,
            ["SKU"] = command.SKU,
            ["UserId"] = command.UserId
        });

        _logger.LogInformation(
            LowStockAlertUpdateLogEvents.UpdateStarted,
            "Starting update of low stock alert. ProductId: {ProductId}, SKU: {SKU}, UserId: {UserId}",
            command.ProductId, command.SKU, command.UserId);

        try
        {
            var alert = await _unitOfWork.LowStockAlertRepository.GetAsync(
                e => e.ProductId == command.ProductId && e.SKU == command.SKU && e.CreatedBy == command.UserId, ct);

            if (alert is null)
            {
                _logger.LogWarning(
                    LowStockAlertUpdateLogEvents.NotFound,
                    "Low stock alert not found for update. ProductId: {ProductId}, SKU: {SKU}, UserId: {UserId}",
                    command.ProductId, command.SKU, command.UserId);
                return LowStockAlertErrors.NotFound(command.ProductId);
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
                    "No changes detected for low stock alert. ProductId: {ProductId}, SKU: {SKU}, UserId: {UserId}",
                    command.ProductId, command.SKU, command.UserId);
                return Result.Success();
            }

            await _unitOfWork.LowStockAlertRepository.UpdateAsync(alert, ct);
            await _unitOfWork.CommitChangesAsync(ct);

            _logger.LogInformation(
                LowStockAlertUpdateLogEvents.UpdateSuccessful,
                "Successfully updated low stock alert. ProductId: {ProductId}, SKU: {SKU}, UserId: {UserId}",
                command.ProductId, command.SKU, command.UserId);

            return Result.Success();
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                LowStockAlertUpdateLogEvents.UpdateCancelled,
                "Low stock alert update was cancelled. ProductId: {ProductId}, SKU: {SKU}, UserId: {UserId}",
                command.ProductId, command.SKU, command.UserId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                LowStockAlertUpdateLogEvents.UpdateFailed,
                ex,
                "Failed to update low stock alert. ProductId: {ProductId}, SKU: {SKU}, UserId: {UserId}",
                command.ProductId, command.SKU, command.UserId);
            return Result.Failure(LowStockAlertErrors.CreationFailed(command.ProductId, command.SKU));
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
