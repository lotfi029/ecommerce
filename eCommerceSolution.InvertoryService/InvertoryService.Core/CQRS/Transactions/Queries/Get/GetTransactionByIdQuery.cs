using InventoryService.Core.DTOs.Transactions;

namespace InventoryService.Core.CQRS.Transactions.Queries.Get;
public record GetTransactionByIdQuery(
    string UserId,
    Guid TransactionId
    ) : IQuery<TransactionResponse>;

public class GetTransactionByIdQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetTransactionByIdQueryHandler> logger
    ) : IQueryHandler<GetTransactionByIdQuery, TransactionResponse>
{
    public async Task<Result<TransactionResponse>> HandleAsync(GetTransactionByIdQuery query, CancellationToken ct = default)
    {
        try
        {
            var transaction = await unitOfWork.TransactionRepository
                .GetAsync(t => t.Id == query.TransactionId && t.CreatedBy == query.UserId, ct);

            if (transaction == null)
                return TransactionErrors.NotFound(query.TransactionId);

            var response = transaction.Adapt<TransactionResponse>();
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get transaction {TransactionId}", query.TransactionId);
            return Error.Unexpected("Failed to get transaction");
        }
    }
}
