using InventoryService.Core.DTOs.Warehouses;
using Mapster;

namespace InventoryService.Core.CQRS.Warehouses.Commands.Add;
public record AddWarehouseCommand(WarehouseRequest Request) : ICommand<Guid>;

public class AddWarehouseCommandHandler(IWarehouseRepository warehouseRepository) : ICommandHandler<AddWarehouseCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(AddWarehouseCommand command, CancellationToken ct = default)
    {
        var warehouse = command.Request.Adapt<Warehouse>();

        try
        {
            var warehouseId = await warehouseRepository.AddAsync(warehouse, ct);

            return warehouseId;
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>(Error.Unexpected(ex.Message));
        }

    }
}
