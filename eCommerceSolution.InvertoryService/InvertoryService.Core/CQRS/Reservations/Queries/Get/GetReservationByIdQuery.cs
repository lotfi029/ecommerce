using InventoryService.Core.DTOs;

namespace InventoryService.Core.CQRS.Reservations.Queries.Get;
public record GetReservationByIdQuery(
    string UserId,
    Guid ReservationId) : IQuery<ReservationResponse>;


public class GetReservationByIdQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetReservationByIdQueryHandler> logger) : IQueryHandler<GetReservationByIdQuery, ReservationResponse>
{
    public async Task<Result<ReservationResponse>> HandleAsync(GetReservationByIdQuery query, CancellationToken ct = default)
    {
        try
        {
            if (await unitOfWork.ReservationRepository.GetAsync(r => r.Id == query.ReservationId && r.CreatedBy == query.UserId, ct) is not { } reservation)
                return ReservationErrors.NotFound(query.ReservationId);

            var response = reservation.Adapt<ReservationResponse>();

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving reservation {ReservationId}", query.ReservationId);
            return Error.Unexpected(ex.Message);
        }
    }
}