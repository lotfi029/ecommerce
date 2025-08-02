using System.Linq.Expressions;

namespace InventoryService.Infrastructure.Presestense.Repositories;
public class WarehouseRepository : Repository<Warehouse>, IWarehouseRepository
{
    private readonly ILogger<WarehouseRepository> _logger;
    public WarehouseRepository(
        ApplicationDbContext context,
        ILogger<Repository<Warehouse>> repositoryLogger,
        ILogger<WarehouseRepository> logger)
        : base(context, repositoryLogger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Warehouse>> GetAllWithFilters(Expression<Func<Warehouse, bool>> filer, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        try
        {
            return await _context.Warehouses
                .AsNoTracking()
                .Where(filer)
                .ToListAsync(ct);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Operation was canceled while retrieving filtered warehouses.");
            return [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving filtered warehouses.");
            throw new Exception("An error occurred while retrieving filtered warehouses.", ex);
        }
    }
}