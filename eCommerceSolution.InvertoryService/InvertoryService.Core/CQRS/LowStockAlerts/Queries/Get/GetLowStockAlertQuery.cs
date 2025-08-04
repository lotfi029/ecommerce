using InventoryService.Core.DTOs.LowStockAlerts;

namespace InventoryService.Core.CQRS.LowStockAlerts.Queries.Get;

public record GetLowStockAlertQuery(string UserId, Guid InventoryId) : IQuery<LowStockAlertResponse>;

public class GetLowStockAlertQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetLowStockAlertQueryHandler> logger) : IQueryHandler<GetLowStockAlertQuery, LowStockAlertResponse>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly ILogger<GetLowStockAlertQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private static readonly EventId EventGettingAlert = new(2001, nameof(EventGettingAlert));
    private static readonly EventId EventAlertNotFound = new(2002, nameof(EventAlertNotFound));
    private static readonly EventId EventAlertFound = new(2003, nameof(EventAlertFound));
    private static readonly EventId EventAlertError = new(2004, nameof(EventAlertError));
    private static readonly EventId EventAlertCanceled = new(2005, nameof(EventAlertCanceled));
    public async Task<Result<LowStockAlertResponse>> HandleAsync(GetLowStockAlertQuery query, CancellationToken ct = default)
    {
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["InventoryId"] = query.InventoryId,
            ["UserId"] = query.UserId
        });

        _logger.LogDebug(EventGettingAlert, "Getting low stock alert.");
        try
        {
            var alert = await _unitOfWork.LowStockAlertRepository.GetAsync(
                e => e.InventoryId == query.InventoryId && e.CreatedBy == query.UserId, ct);

            if (alert is null)
            {
                _logger.LogWarning(EventAlertNotFound, "Low stock alert not found.");
                return Result.Failure<LowStockAlertResponse>(LowStockAlertErrors.NotFound(query.InventoryId));
            }

            var response = alert.Adapt<LowStockAlertResponse>();

            _logger.LogInformation(EventAlertFound, "Low stock alert found.");
            return Result.Success(response);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(EventAlertCanceled, "Low stock alert query was cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(EventAlertError, ex, "Error getting low stock alert.");
            return Result.Failure<LowStockAlertResponse>(Error.Unexpected(ex.Message));
        }
    }
}