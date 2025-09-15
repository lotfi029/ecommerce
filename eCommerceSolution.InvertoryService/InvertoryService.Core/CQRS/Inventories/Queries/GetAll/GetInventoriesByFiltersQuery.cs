using InventoryService.Core.DTOs.Inventories;

namespace InventoryService.Core.CQRS.Inventories.Queries.GetAll;
public record GetInventoriesByFiltersQuery(
    string UserId,
    Guid? WarehouseId,
    Guid? ProductId,
    string? SKU,
    int? ReorderLevel,
    bool IsReorderLevel = false) : IQuery<IEnumerable<InventoryResponse>>;


public class GetInventoriesByFiltersQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetInventoriesByFiltersQueryHandler> logger) : IQueryHandler<GetInventoriesByFiltersQuery, IEnumerable<InventoryResponse>>
{
    public async Task<Result<IEnumerable<InventoryResponse>>> HandleAsync(GetInventoriesByFiltersQuery query, CancellationToken ct = default)
    {
        try
        {
            var inventories = await unitOfWork.InventoryRepository
                .GetAllAsync(i => 
                    i.CreatedBy == query.UserId && 
                    (string.IsNullOrEmpty(query.SKU) || i.SKU == query.SKU) &&
                    (query.ProductId == null || query.ProductId == i.ProductId) && 
                    (query.ReorderLevel == null || query.ReorderLevel == i.ReorderLevel) &&
                    (query.WarehouseId == null || query.WarehouseId == i.WarehouseId) &&
                    (query.IsReorderLevel && i.ReorderLevel >= i.Quantity),
                    ct
                );

            if (!inventories.Any())
                return Result.Success(Enumerable.Empty<InventoryResponse>());

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
