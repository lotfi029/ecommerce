

using System.Linq.Expressions;


namespace InventoryService.Infrastructure.Presestense.Repository;

public class LowStockAlertRepository(
    ApplicationDbContext _context,
    ILogger<LowStockAlertRepository> _logger) : ILowStockAlertRepository
{
    public Task<Guid> AddAsync(LowStockAlert entity, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task AddRangeAsync(IEnumerable<LowStockAlert> entities, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LowStockAlert>> GetAllAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LowStockAlert>> GetAllWithFilters(Expression<Func<LowStockAlert, bool>> filer, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<LowStockAlert?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(LowStockAlert entity, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
