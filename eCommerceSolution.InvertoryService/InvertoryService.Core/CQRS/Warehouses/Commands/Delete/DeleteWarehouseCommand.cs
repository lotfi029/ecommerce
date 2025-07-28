namespace InventoryService.Core.CQRS.Warehouses.Commands.Delete;
public record DeleteWarehouseCommand(string UserId, Guid Id) : ICommand;

public class DeleteWarehouseCommandHandler(IWarehouseRepository warehouseRepository) : ICommandHandler<DeleteWarehouseCommand>
{
    public async Task<Result> HandleAsync(DeleteWarehouseCommand command, CancellationToken ct = default)
    {
        if (await warehouseRepository.GetByIdAsync(command.Id, ct) is not { } warehouse)
            return WarehouseErrors.NotFound(command.Id);
        if (warehouse.CreatedBy != command.UserId)
            return WarehouseErrors.InvalidAccess(command.UserId);
        try
        {
            await warehouseRepository.DeleteAsync(command.Id, ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Unexpected(ex.Message));
        }
    }
}
