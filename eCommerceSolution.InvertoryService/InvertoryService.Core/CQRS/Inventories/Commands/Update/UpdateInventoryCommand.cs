namespace InventoryService.Core.CQRS.Inventories.Commands.Update;

public record UpdateInventoryCommand(
    string UserId,
    Guid ProductId,
    int Quantity,
    bool IsReservation) : ICommand;

public class UpdateInventoryCommandHandler(
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateInventoryCommand>
{
    public async Task<Result> HandleAsync(UpdateInventoryCommand command, CancellationToken ct = default)
    {
        try
        {
            var inventory = await unitOfWork.InventoryRepository.GetByIdAsync(command.ProductId, ct);
            
            if (inventory is null)
                return Result.Failure(Error.NotFound("Inventory.NotFound", $"Inventory with ProductId {command.ProductId} not found."));

            //if (command.IsReservation)
            //{
            //    if (inventory.Quantity - inventory.Reserved < command.Quantity)
            //        return Result.Failure(Error.BadRequest("Inventory.InsufficientStock", "Not enough stock available for reservation."));

            //    inventory.Reserved += command.Quantity;
            //}
            //else
            //{
            //    inventory.Reserve(command.Quantity);

            //    if (inventory.Quantity < inventory.Reserved)
            //        return Result.Failure(Error.BadRequest("Inventory.InsufficientStock", "Cannot reduce quantity below reserved amount."));
            //}

            await unitOfWork.InventoryRepository.UpdateAsync(inventory, ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Unexpected(ex.Message));
        }
    }   
}