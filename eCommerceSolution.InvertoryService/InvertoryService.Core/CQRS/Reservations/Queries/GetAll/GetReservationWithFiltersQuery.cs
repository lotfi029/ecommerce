using InventoryService.Core.DTOs;

namespace InventoryService.Core.CQRS.Reservations.Queries.GetAll;
public record GetReservationWithFiltersQuery(
    string UserId,
    Guid? InventoryId,
    Guid? OrderId
    ) : IQuery<IEnumerable<ReservationResponse>>;


public class GetReservationWithFiltersQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetReservationWithFiltersQueryHandler> logger) : IQueryHandler<GetReservationWithFiltersQuery, IEnumerable<ReservationResponse>>
{
    public async Task<Result<IEnumerable<ReservationResponse>>> HandleAsync(GetReservationWithFiltersQuery query, CancellationToken ct = default)
    {
        try
        {
            var reservations = await unitOfWork.ReservationRepository
                .GetAllAsync(
                    r => r.CreatedBy == query.UserId &&
                    (query.InventoryId == null || r.InventoryId == query.InventoryId) &&
                    (query.OrderId == null || r.OrderId == query.OrderId),
                    ct
                );

            if (!reservations.Any())
                return Result.Success(Enumerable.Empty<ReservationResponse>());

            var response = reservations.Adapt<IEnumerable<ReservationResponse>>();

            return Result.Success(response);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving reservations with filters InventoryId {InventoryId} and OrderId {OrderId}", query.InventoryId, query.OrderId);
            return Error.Unexpected(ex.Message);
        }
    }
}