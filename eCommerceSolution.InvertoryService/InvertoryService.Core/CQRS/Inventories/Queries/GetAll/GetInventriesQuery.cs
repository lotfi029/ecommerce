using InventoryService.Core.DTOs.Inventories;

namespace InventoryService.Core.CQRS.Inventories.Queries.GetAll;
public record GetInventriesQuery(string UserId) : IQuery<List<InventoryResponse>>;

public class GetInventriesQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetInventriesQueryHandler> logger) : IQueryHandler<GetInventriesQuery, List<InventoryResponse>>
{
    public async Task<Result<List<InventoryResponse>>> HandleAsync(GetInventriesQuery query, CancellationToken ct = default)
    {
        try
        {
            var inventories = await unitOfWork.InventoryRepository
                .GetAllAsync(i => i.CreatedBy == query.UserId, ct);

            var response = inventories.Adapt<List<InventoryResponse>>();

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving inventories for UserId {UserId}", query.UserId);
            return Error.Unexpected(ex.Message);
        }
    }
}