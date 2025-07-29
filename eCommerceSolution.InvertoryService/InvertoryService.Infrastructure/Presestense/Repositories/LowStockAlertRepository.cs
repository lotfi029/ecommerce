using System.Linq.Expressions;

namespace InventoryService.Infrastructure.Presestense.Repositories;

public class LowStockAlertRepository(
    ApplicationDbContext context,
    ILogger<Repository<LowStockAlert>> repositoryLogger,
    ILogger<LowStockAlertRepository> logger) : Repository<LowStockAlert>(context,repositoryLogger), ILowStockAlertRepository
{
    public Task<IEnumerable<LowStockAlert>> GetAllWithFilters(Expression<Func<LowStockAlert, bool>> filer, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
