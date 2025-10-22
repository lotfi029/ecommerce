using InventoryService.Core.DTOs.Transactions;

namespace InventoryService.Core.CQRS.Transactions.Queries.GetAll;
public record GetTransactionsQuery(
    string UserId,
    Guid? InventoryId,
    InventoryChangeType? ChangeType,
    DateTime? From,
    DateTime? To ) : IQuery<IEnumerable<TransactionResponse>>;


public class GetTransactionsQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetTransactionsQueryHandler> logger
    ) : IQueryHandler<GetTransactionsQuery, IEnumerable<TransactionResponse>>
{
    public async Task<Result<IEnumerable<TransactionResponse>>> HandleAsync(GetTransactionsQuery query, CancellationToken ct = default)
    {
        try
        {
            var transactions = await unitOfWork.TransactionRepository
                .GetAllAsync(t => 
                    t.CreatedBy == query.UserId &&
                    (query.InventoryId == null || query.InventoryId == t.InventoryId) &&
                    (query.ChangeType == null || query.ChangeType == t.ChangeType) &&
                    (query.From == null || query.From >= t.CreatedAt) &&
                    (query.To == null || query.To <= t.CreatedAt),
                    ct
                );

            if (!transactions.Any())
                return Result.Success(Enumerable.Empty<TransactionResponse>());

            var response = transactions.Adapt<IEnumerable<TransactionResponse>>();

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get transactions");
            return Error.Unexpected("Failed to get transactions");
        }
    }
}
