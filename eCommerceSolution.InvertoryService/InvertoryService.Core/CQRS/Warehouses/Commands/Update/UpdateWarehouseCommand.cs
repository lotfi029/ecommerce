using InventoryService.Core.DTOs.Warehouses;

namespace InventoryService.Core.CQRS.Warehouses.Commands.Update;
public record UpdateWarehouseCommand(string UserId, Guid Id, WarehouseRequest Request) : ICommand;

public record UpdateWarehouseCommandHandler(IWarehouseRepository warehouseRepository) : ICommandHandler<UpdateWarehouseCommand>
{
    public async Task<Result> HandleAsync(UpdateWarehouseCommand command, CancellationToken ct = default)
    {
        if (await warehouseRepository.GetByIdAsync(command.Id, ct) is not { } warehouse)
            return WarehouseErrors.NotFound(command.Id);

        if (warehouse.CreatedBy != command.UserId)
            return WarehouseErrors.InvalidAccess(command.UserId);

        warehouse.Name = command.Request.Name;
        warehouse.Location = command.Request.Location;

        try
        {
            await warehouseRepository.UpdateAsync(warehouse, ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Unexpected(ex.Message));
        }
    }
}
