namespace InventoryService.Core.CQRS.LowStockAlerts.Commands.Update;
public record UpdateLowStockAlertCommand(string UserId, Guid ProductId, string SKU) : ICommand;

public class UpdateLowStockAlertCommandHandler : ICommandHandler<UpdateLowStockAlertCommand>
{
    public Task<Result> HandleAsync(UpdateLowStockAlertCommand command, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
