using InventoryService.Core.DTOs.Warehouses;

namespace InventoryService.Core.CQRS.Warehouses.Queries.GetAll;
public record GetAllWarehouseQuery(string UserId) : IQuery<IEnumerable<WarehouseResponse>>;

public class GetAllWarehouseQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetAllWarehouseQuery, IEnumerable<WarehouseResponse>>
{
    public async Task<Result<IEnumerable<WarehouseResponse>>> HandleAsync(GetAllWarehouseQuery query, CancellationToken ct = default)
    {
        var warehouses = await unitOfWork.WarehouseRepository.GetAllWithFilters(e => e.CreatedBy == query.UserId, ct);
        
        var response = warehouses.Adapt<IEnumerable<WarehouseResponse>>();

        return Result.Success(response);
    }
}