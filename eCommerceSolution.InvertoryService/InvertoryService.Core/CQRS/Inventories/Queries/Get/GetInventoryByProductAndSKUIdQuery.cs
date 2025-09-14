using InventoryService.Core.DTOs.Inventories;

namespace InventoryService.Core.CQRS.Inventories.Queries.Get;
public record GetInventoryByProductAndSKUIdQuery(
    string UserId,
    string SKU,
    Guid ProductId
    ) : IQuery<InventoryResponse>;

public class GetInventoryByProductAndSKUIdQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetInventoryByProductAndSKUIdQueryHandler> logger) : IQueryHandler<GetInventoryByProductAndSKUIdQuery, InventoryResponse>
{
    public async Task<Result<InventoryResponse>> HandleAsync(GetInventoryByProductAndSKUIdQuery query, CancellationToken ct = default)
    {
        try
        {
            var inventory = await unitOfWork.InventoryRepository
                .GetAsync(i => i.ProductId == query.ProductId && i.SKU == query.SKU && i.CreatedBy == query.UserId, ct);

            if (inventory is null)
                return Error.NotFound("Inventory.NotFound", $"Inventory with ProductId {query.ProductId} and SKU {query.SKU} not found.");
            
            var response = new InventoryResponse(
                Id: inventory.Id,
                SKU: inventory.SKU,
                ProductId: inventory.ProductId,
                Quantity: inventory.Quantity,
                ReorderLevel: inventory.ReorderLevel,
                WarehouseId: inventory.WarehouseId,
                CreatedAt: inventory.CreatedAt,
                UpdatedAt: inventory.UpdatedAt ?? default
            );

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving inventory for ProductId {ProductId} and SKU {SKU}", query.ProductId, query.SKU);
            return Error.Unexpected(ex.Message);
        }
    }
}