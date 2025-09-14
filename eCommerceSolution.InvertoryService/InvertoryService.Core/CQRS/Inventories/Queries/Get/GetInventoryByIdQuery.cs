using InventoryService.Core.DTOs.Inventories;

namespace InventoryService.Core.CQRS.Inventories.Queries.Get;
public record GetInventoryByIdQuery(
    string UserId,
    Guid InventoryId
    ) : IQuery<InventoryResponse>;

public class GetInventoryByIdQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetInventoryByIdQueryHandler> logger) : IQueryHandler<GetInventoryByIdQuery, InventoryResponse>
{
    public async Task<Result<InventoryResponse>> HandleAsync(GetInventoryByIdQuery query, CancellationToken ct = default)
    {
        try
        {
            if (await unitOfWork.InventoryRepository.FindAsync(ct, query.InventoryId) is not { } inventory)
                return Error.NotFound("Inventory.NotFound", $"Inventory with Id {query.InventoryId} not found.");

            var response = inventory.Adapt<InventoryResponse>();

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving inventory for InventoryId {InventoryId}", query.InventoryId);
            return Error.Unexpected(ex.Message);
        }
    }
}
