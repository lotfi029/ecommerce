namespace InventoryService.Core.CQRS.Inventories.Commands.Update;

public record UpdateReorderLevelCommand(
    string UserId,
    Guid InventoryId,
    int ReorderLevel) : ICommand;

public class UpdateReorderLevelCommandHandler(
    IUnitOfWork unitOfWork,
    ILogger<UpdateReorderLevelCommandHandler> logger) : ICommandHandler<UpdateReorderLevelCommand>
{
    public async Task<Result> HandleAsync(UpdateReorderLevelCommand command, CancellationToken ct = default)
    {
        if (await unitOfWork.InventoryRepository.FindAsync(ct, command.InventoryId) is not { } inventory) 
            return Result.Failure(Error.NotFound("Inventory.NotFound", $"Inventory with Id {command.InventoryId} not found."));
        inventory.ReorderLevel = command.ReorderLevel;
        try
        {            
            await unitOfWork.InventoryRepository.UpdateAsync(inventory, ct);
            await unitOfWork.CommitChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating reorder level for inventory {InventoryId}", command.InventoryId);
            return Error.Unexpected(ex.Message);
        }
    }   
}