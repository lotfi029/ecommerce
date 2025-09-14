using InventoryService.Core.DTOs.Warehouses;

namespace InventoryService.Core.CQRS.Warehouses.Queries.Get;
public record GetWarehouseByIdQuery(string UserId, Guid Id) : IQuery<WarehouseResponse>;

public class GetWarehouseByIdQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetWarehouseByIdQuery, WarehouseResponse>
{
    public async Task<Result<WarehouseResponse>> HandleAsync(GetWarehouseByIdQuery query, CancellationToken ct = default)
    {
        if (await unitOfWork.WarehouseRepository.GetAsync(e => e.Id == query.Id && e.CreatedBy == query.UserId, ct) is not { } warehouse)
            return WarehouseErrors.NotFound(query.Id);
        
        var response = warehouse.Adapt<WarehouseResponse>();

        return response;
    }
}