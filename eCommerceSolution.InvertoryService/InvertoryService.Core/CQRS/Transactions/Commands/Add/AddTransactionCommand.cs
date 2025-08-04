using InventoryService.Core.DTOs.Transactions;

namespace InventoryService.Core.CQRS.Transactions.Commands.Add;
public record AddTransactionCommand(string UserId, TransactionRequest Request) : ICommand<Guid>;

public class AddTransactionCommandHandler(
    IUnitOfWork unitOfWork, 
    ILogger<AddTransactionCommandHandler> logger
    ) : ICommandHandler<AddTransactionCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly ILogger<AddTransactionCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private static readonly EventId EventTransactionAdded = new(1001, nameof(EventTransactionAdded));
    private static readonly EventId EventTransactionAddError = new(1002, nameof(EventTransactionAddError));
    private static readonly EventId EventTransactionValidationError = new(1003, nameof(EventTransactionValidationError));
    private static readonly EventId EventTransactionCanceled = new(1004, nameof(EventTransactionCanceled));
    private static readonly EventId EventTransactionAlreadyExist = new(1005, nameof(EventTransactionAlreadyExist));

    public Task<Result<Guid>> HandleAsync(AddTransactionCommand request, CancellationToken cancellationToken)
    {
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = request.UserId,
            ["ProductId"] = request.Request.ProductId,
            ["SKU"] = request.Request.SKU,
            ["ChangeType"] = request.Request.ChangeType,
            ["QuantityChanged"] = request.Request.QuantityChanged,
            ["OrderId"] = request.Request.OrderId ?? Guid.Empty
        });

        _logger.LogDebug(EventTransactionAdded, "Adding transaction.");
        
        throw new NotImplementedException("Transaction addition logic is not implemented yet.");
    }
}