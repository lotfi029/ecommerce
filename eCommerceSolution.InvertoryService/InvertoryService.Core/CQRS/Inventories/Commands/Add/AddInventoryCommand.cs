namespace InventoryService.Core.CQRS.Inventories.Commands.Add;
public record AddInventoryCommand(string UserId, Guid ProductId, int Quantity) : ICommand<Guid>;

public class AddInventoryCommandHandler(
    IInventoryRepository inventoryRepository) : ICommandHandler<AddInventoryCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(AddInventoryCommand command, CancellationToken ct = default)
    {
        try
        {
            if (await inventoryRepository.ExistsAsync(command.ProductId, ct))
                return InventoryErrors.AlreadyExists(command.ProductId);

            if (command.Quantity <= 0)
                return InventoryErrors.InvalidQuantity(command.Quantity);

            var inventory = Inventory.Create(
                command.ProductId,
                command.Quantity,
                command.UserId
            );

            var inventoryId = await inventoryRepository.AddAsync(inventory, ct);

            if (inventoryId == Guid.Empty)
                return InventoryErrors.InventoryCreationFailed(command.ProductId);

            return inventoryId;

        }
        catch (Exception ex)
        {
            return Error.Unexpected(ex.Message);
        }
    }
}
