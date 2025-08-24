namespace InventoryService.Core.CQRS.Inventories.Commands.Add;
public record AddInventoryCommand(string UserId, Guid ProductId, int Quantity, string SKU, Guid Warehouse) : ICommand<Guid>;

public class AddInventoryCommandHandler(
    IUnitOfWork unitOfWork) : ICommandHandler<AddInventoryCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(AddInventoryCommand command, CancellationToken ct = default)
    {
        try
        {
            if (await unitOfWork.InventoryRepository.ExistsAsync(command.ProductId, ct))
                return InventoryErrors.AlreadyExists(command.ProductId);

            if (command.Quantity <= 0)
                return InventoryErrors.InvalidQuantity(command.Quantity);

            var inventory = new Inventory(
                command.ProductId,
                command.SKU,
                command.Quantity,
                command.Warehouse,
                command.UserId
            );

            var inventoryId = await unitOfWork.InventoryRepository.AddAsync(inventory, ct);

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
