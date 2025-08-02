using System.Linq.Expressions;

namespace InventoryService.Infrastructure.Presestense.Repositories;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    private readonly ILogger<TransactionRepository> _logger;

    public TransactionRepository(
        ApplicationDbContext context,
        ILogger<Repository<Transaction>> repositoryLogger,
        ILogger<TransactionRepository> logger
        ) : base(context, repositoryLogger)
    {
        _context = context;
        _logger = logger;
    }
    public Task<IEnumerable<Transaction>> GetAllWithFilters(Expression<Func<Transaction, bool>> filter, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Transaction>> GetByProductIdAsync(Guid productId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Transaction>> GetBySKUAsync(string sku, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
