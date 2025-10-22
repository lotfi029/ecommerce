using InventoryService.Core.DTOs.Transactions;

namespace InventoryService.Core.CQRS.Transactions.Commands.Add;
public record AddTransactionCommand(
    string UserId, 
    Guid InventoryId, 
    InventoryChangeType ChangeType,
    TransactionRequest Request) : ICommand<Guid>;

public class AddTransactionCommandHandler(
    IUnitOfWork unitOfWork, 
    ILogger<AddTransactionCommandHandler> logger
    ) : ICommandHandler<AddTransactionCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(AddTransactionCommand command, CancellationToken ct)
    {
        try
        {
            if (await unitOfWork.InventoryRepository.FindAsync(ct, command.InventoryId) is not { } inventory)
                return TransactionErrors.NotFound(command.InventoryId);

            if (command.ChangeType == InventoryChangeType.OUT && inventory.Quantity < command.Request.QuantityChanged)
                return TransactionErrors.InsufficientInventory(command.InventoryId, inventory.Quantity, command.Request.QuantityChanged);

            var transaction = new Transaction
            {
                InventoryId = command.InventoryId,
                ChangeType = command.ChangeType,
                QuantityChanged = command.Request.QuantityChanged,
                Reason = command.Request.Reason
            };

            var beginTransaction = await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                switch (command.ChangeType)
                {
                    case InventoryChangeType.RETURN:
                    case InventoryChangeType.IN:
                        inventory.Quantity += command.Request.QuantityChanged;
                        break;

                    case InventoryChangeType.OUT:
                        inventory.Quantity -= command.Request.QuantityChanged;
                        break;

                    default:
                        return TransactionErrors.InvalidTransactionType(command.ChangeType.ToString());
                }

                await unitOfWork.TransactionRepository.AddAsync(transaction, ct);
                await unitOfWork.InventoryRepository.UpdateAsync(inventory, ct);
                await unitOfWork.CommitTransactionAsync(beginTransaction, ct);
                return transaction.Id;

            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(beginTransaction, ct);
                logger.LogError(ex, "Failed to create transaction for inventory {InventoryId}", command.InventoryId);
                throw;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while adding transaction for InventoryId: {InventoryId}", command.InventoryId);
            return Error.Unexpected("Failed to complete the trunsactions");
        }
    }
}