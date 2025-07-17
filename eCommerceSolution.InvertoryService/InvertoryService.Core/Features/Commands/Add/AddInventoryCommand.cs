using eCommerce.SharedKernal.Abstractions;
using eCommerce.SharedKernal.Messaging;

namespace InventoryService.Core.Features.Commands.Add;
public record AddInventoryCommand(Guid ProductId, int Quantity) : ICommand;

public class AddInventoryCommandHandler : ICommandHandler<AddInventoryCommand>
{
    public Task<Result> HandleAsync(AddInventoryCommand command, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
