using InventoryService.Core.DTOs.Warehouses;

namespace InventoryService.Core.CQRS.Warehouses.Commands.Update;
public record UpdateWarehouseCommand(string UserId, Guid Id, WarehouseRequest Request) : ICommand;

public class UpdateWarehouseCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateWarehouseCommand>
{
    public async Task<Result> HandleAsync(UpdateWarehouseCommand command, CancellationToken ct = default)
    {
        try
        {
            if (await unitOfWork.WarehouseRepository.GetByIdAsync(command.Id, ct) is not { } warehouse)
                return WarehouseErrors.NotFound(command.Id);

            if (warehouse.CreatedBy != command.UserId)
                return WarehouseErrors.InvalidAccess(command.UserId);

            warehouse.Name = command.Request.Name;
            warehouse.Location = command.Request.Location;

            await unitOfWork.WarehouseRepository.UpdateAsync(warehouse, ct);
            await unitOfWork.CommitChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Unexpected(ex.Message));
        }
    }
}
