using InventoryService.Core.DTOs.LowStockAlerts;
using Microsoft.Extensions.Logging;

namespace InventoryService.Core.CQRS.LowStockAlerts.Queries.GetAll;
public record GetAllLowStockAlertQuery(string UserId) : IQuery<IEnumerable<LowStockAlertResponse>>;

public class GetAllLowStockAlertQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetAllLowStockAlertQueryHandler> logger) : IQueryHandler<GetAllLowStockAlertQuery, IEnumerable<LowStockAlertResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly ILogger<GetAllLowStockAlertQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private static readonly EventId EventGettingAlerts = new(3001, nameof(EventGettingAlerts));
    private static readonly EventId EventAlertsNotFound = new(3002, nameof(EventAlertsNotFound));
    private static readonly EventId EventAlertsFound = new(3003, nameof(EventAlertsFound));
    private static readonly EventId EventAlertsError = new(3004, nameof(EventAlertsError));
    private static readonly EventId EventAlertsCanceled = new(3005, nameof(EventAlertsCanceled));


    public async Task<Result<IEnumerable<LowStockAlertResponse>>> HandleAsync(GetAllLowStockAlertQuery query, CancellationToken ct = default)
    {
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = query.UserId
        });
        _logger.LogDebug(EventGettingAlerts, "Getting all low stock alerts.");
        try
        {
            var alerts = await _unitOfWork.LowStockAlertRepository.GetAllWithFilters(
                e => e.CreatedBy == query.UserId, ct);

            if (alerts is null || !alerts.Any())
            {
                _logger.LogWarning(EventAlertsNotFound, "No low stock alerts found for user {UserId}.", query.UserId);
                return LowStockAlertErrors.NotFound();
            }

            var responses = alerts.Adapt<IEnumerable<LowStockAlertResponse>>();
            _logger.LogInformation(EventAlertsFound, "Low stock alerts found for user {UserId}.", query.UserId);
            return Result.Success(responses);

        }
        catch(OperationCanceledException)
        {
            _logger.LogWarning(EventAlertsCanceled, "Low stock alerts query was cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(EventAlertsError, ex, "Error getting low stock alerts.");
            return Error.Unexpected(ex.Message);
        }
    }
}
