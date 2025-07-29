using System.Linq.Expressions;

namespace InventoryService.Infrastructure.Presestense.Repositories;

public class InventoryRepository(
    ApplicationDbContext context,
    ILogger<Repository<Inventory>> repositoryLogger,
    ILogger<InventoryRepository> _logger) : Repository<Inventory>(context, repositoryLogger), IInventoryRepository
{

    public async Task<Inventory> GetAllWithFilter(Expression<Func<Inventory, bool>> expression, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(expression, nameof(expression));
        try
        {
            if (_context.Invertories is null)
            {
                _logger.LogError("Invertories DbSet is null");
                throw new InvalidOperationException("Invertories DbSet is not initialized");
            }
            
            var result = await _context.Invertories
                .AsNoTracking()
                .FirstOrDefaultAsync(expression, ct);

            if (result is null)
            {
                _logger.LogWarning("No inventory found matching the specified filter");
                return default!;
            }

            _logger.LogInformation("Successfully retrieved inventory with ID {InventoryId}", result.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving inventory with filter");
            throw new InvalidOperationException("Error retrieving inventory with filter", ex);
        }
    }
}
