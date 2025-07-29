namespace InventoryService.Core.CQRS.LowStockAlerts.Commands.Add;
public record AddLowStockAlertCommand(string UserId, Guid ProductId, string SKU) : ICommand<Guid>;

public class AddLowStockAlertCommandHandler : ICommandHandler<AddLowStockAlertCommand, Guid>
{
    public Task<Result<Guid>> HandleAsync(AddLowStockAlertCommand command, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}