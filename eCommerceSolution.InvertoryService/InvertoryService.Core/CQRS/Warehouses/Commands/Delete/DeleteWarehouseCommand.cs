namespace InventoryService.Core.CQRS.Warehouses.Commands.Delete;
public record DeleteWarehouseCommand(string UserId, Guid Id) : ICommand;

public class DeleteWarehouseCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteWarehouseCommand>
{
    public async Task<Result> HandleAsync(DeleteWarehouseCommand command, CancellationToken ct = default)
    {
        if (await unitOfWork.WarehouseRepository.GetByIdAsync(command.Id, ct) is not { } warehouse)
            return WarehouseErrors.NotFound(command.Id);
        if (warehouse.CreatedBy != command.UserId)
            return WarehouseErrors.InvalidAccess(command.UserId);
        try
        {
            await unitOfWork.WarehouseRepository.DeleteAsync(command.Id, ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Unexpected(ex.Message));
        }
    }
}
