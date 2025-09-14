using InventoryService.Core.DTOs.Inventories;

namespace InventoryService.Core.CQRS.Inventories.Queries.GetAll;
public record GetInventoriesBySKUQuery(
    string UserId,
    string SKU) : IQuery<List<InventoryResponse>>;


public class GetInventoriesBySKUQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetInventoriesBySKUQueryHandler> logger) : IQueryHandler<GetInventoriesBySKUQuery, List<InventoryResponse>>
{
    public async Task<Result<List<InventoryResponse>>> HandleAsync(GetInventoriesBySKUQuery query, CancellationToken ct = default)
    {
        try
        {
            var inventories = await unitOfWork.InventoryRepository
                .GetAllAsync(i => i.CreatedBy == query.UserId && i.SKU == query.SKU, ct);

            var response = inventories.Adapt<List<InventoryResponse>>();
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving inventories");
            return Error.Unexpected(ex.Message);
        }
    }
}
