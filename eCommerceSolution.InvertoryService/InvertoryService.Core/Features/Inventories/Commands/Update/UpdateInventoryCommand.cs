namespace InventoryService.Core.Features.Inventories.Commands.Update;

public record UpdateInventoryCommand(
    string UserId,
    Guid ProductId,
    int Quantity,
    bool IsReservation) : ICommand;

public class UpdateInventoryCommandHandler(
    IInventoryRepository inventoryRepository) : ICommandHandler<UpdateInventoryCommand>
{
    public async Task<Result> HandleAsync(UpdateInventoryCommand command, CancellationToken ct = default)
    {
        try
        {
            var inventory = await inventoryRepository.GetByIdAsync(command.ProductId, ct);
            
            if (inventory is null)
                return Result.Failure(Error.NotFound("Inventory.NotFound", $"Inventory with ProductId {command.ProductId} not found."));

            if (command.IsReservation)
            {
                if (inventory.Quantity - inventory.Reserved < command.Quantity)
                    return Result.Failure(Error.BadRequest("Inventory.InsufficientStock", "Not enough stock available for reservation."));

                inventory.Reserved += command.Quantity;
            }
            else
            {
                var updatedInventory = Inventory.UpdateQuantity(inventory, command.Quantity);
                
                if (updatedInventory.Quantity < updatedInventory.Reserved)
                    return Result.Failure(Error.BadRequest("Inventory.InsufficientStock", "Cannot reduce quantity below reserved amount."));
                
                inventory = updatedInventory;
            }

            await inventoryRepository.UpdateAsync(inventory, ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Unexpected(ex.Message));
        }
    }   
}