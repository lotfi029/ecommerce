using InventoryService.Core.DTOs.Warehouses;

namespace InventoryService.Core.CQRS.Warehouses.Queries.Get;
public record GetWarehouseByIdQuery(string UserId, Guid Id) : IQuery<WarehouseResponse>;

public class GetWarehouseByIdQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetWarehouseByIdQuery, WarehouseResponse>
{
    public async Task<Result<WarehouseResponse>> HandleAsync(GetWarehouseByIdQuery query, CancellationToken ct = default)
    {
        if (await unitOfWork.WarehouseRepository.GetByIdAsync(query.Id, ct) is not { } warehouse)
            return WarehouseErrors.NotFound(query.Id);
        if (warehouse.CreatedBy != query.UserId)
            return WarehouseErrors.InvalidAccess(query.UserId);

        var response = warehouse.Adapt<WarehouseResponse>();

        return response;
    }
}