namespace InventoryService.Core.CQRS.Inventories.Commands.Update;

public record UpdateReorderLevelCommand(
    string UserId,
    Guid InventoryId,
    int ReorderLevel) : ICommand;

public class UpdateReorderLevelCommandHandler(
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateReorderLevelCommand>
{
    public async Task<Result> HandleAsync(UpdateReorderLevelCommand command, CancellationToken ct = default)
    {
        try
        {
            if (await unitOfWork.InventoryRepository.FindAsync(ct, command.InventoryId) is not { } inventory) 
                return Result.Failure(Error.NotFound("Inventory.NotFound", $"Inventory with Id {command.InventoryId} not found."));
            
            inventory.ReorderLevel = command.ReorderLevel;

            await unitOfWork.InventoryRepository.UpdateAsync(inventory, ct);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Unexpected(ex.Message));
        }
    }   
}