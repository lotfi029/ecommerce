namespace InventoryService.Core.CQRS.LowStockAlerts.Commands.Delete;
public record DeleteLowStockAlertCommand(string UserId, Guid ProductId, string SKU) : ICommand;

public class DeleteLowStockAlertCommandHandler : ICommandHandler<DeleteLowStockAlertCommand>
{
    public Task<Result> HandleAsync(DeleteLowStockAlertCommand command, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}