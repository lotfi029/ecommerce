using System.Linq.Expressions;

namespace InventoryService.Infrastructure.Presestense.Repositories;

public class WarehouseRepository(
    ApplicationDbContext context,
    ILogger<Repository<Warehouse>> repositoryLogger,
    ILogger<WarehouseRepository> logger) : Repository<Warehouse>(context, repositoryLogger), IWarehouseRepository
{
    public async Task<IEnumerable<Warehouse>> GetAllWithFilters(Expression<Func<Warehouse, bool>> filer, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        try
        {
            return await context.Warehouses
                .AsNoTracking()
                .Where(filer)
                .ToListAsync(ct);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("Operation was canceled while retrieving filtered warehouses.");
            return [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving filtered warehouses.");
            throw new Exception("An error occurred while retrieving filtered warehouses.", ex);
        }
    }
}