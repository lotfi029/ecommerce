namespace InventoryService.Core.CQRS.Inventories.Commands.Add;
public record AddInventoryCommand(
    Guid ProductId, 
    int Quantity, 
    string SKU, 
    Guid WarehouseId,
    int ReorderLevel = 0) : ICommand<Guid>;

public class AddInventoryCommandHandler(
    IUnitOfWork unitOfWork) : ICommandHandler<AddInventoryCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(AddInventoryCommand command, CancellationToken ct = default)
    {
        try
        {
            if (await unitOfWork.InventoryRepository.ExistsAsync(e => e.ProductId == command.ProductId && e.SKU == command.SKU, ct))
                return InventoryErrors.AlreadyExists(command.ProductId);

            if (command.Quantity <= 0)
                return InventoryErrors.InvalidQuantity(command.Quantity);

            var inventory = new Inventory(
                productId: command.ProductId,
                sku: command.SKU,
                quantity: command.Quantity,
                reorderLevel: command.ReorderLevel,
                warehouseId: command.WarehouseId
            );

            var inventoryId = await unitOfWork.InventoryRepository.AddAsync(inventory, ct);

            if (inventoryId == Guid.Empty)
                return InventoryErrors.InventoryCreationFailed(command.ProductId);

            return inventoryId;
        }
        catch (Exception ex)
        {
            // TODO: Log exception
            return Error.Unexpected(ex.Message);
        }
    }
}
