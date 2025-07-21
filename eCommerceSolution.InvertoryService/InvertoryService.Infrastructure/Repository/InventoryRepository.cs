using InventoryService.Core.IRepositories;
using InventoryService.Infrastructure.Presestense;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryService.Infrastructure.Repository;
public class InventoryRepository(
    ApplicationDbContext _context,
    ILogger<InventoryRepository> _logger) : IInventoryRepository
{
    public async Task<Guid> AddAsync(Inventory entity, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        try
        {
            await _context.Invertories.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Entity added successfully with ID: {Id}", entity.Id);
            return entity.Id;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Operation was canceled while adding entity with ID: {Id}", entity.Id);
            return Guid.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding entity with ID: {Id}", entity.Id);
            throw new Exception("An error occurred while adding the entity.", ex);
        }
    }

    public async Task AddRangeAsync(IEnumerable<Inventory> entities, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            await _context.Invertories.AddRangeAsync(entities, ct);
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Entities added successfully");
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Operation was canceled while adding entities.");
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding entities.", ex);
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid Invertory ID", nameof(id));

        try
        {
            ct.ThrowIfCancellationRequested();

            var rowsDeleted = await _context.Invertories
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync(ct);

            _logger.LogInformation("Invertory {InvertoryId} deleted successfully", id);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Delete operation was cancelled for Invertory {InvertoryId}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Invertory {InvertoryId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
            return false;

        try
        {
            ct.ThrowIfCancellationRequested();

            return await _context.Invertories
                .AnyAsync(p => p.Id == id, ct);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Exists check was cancelled for Invertory {InvertoryId}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if Invertory {InvertoryId} exists", id);
            throw;
        }
    }

    public async Task<IEnumerable<Inventory>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var Invertorys = await _context.Invertories
                .AsNoTracking()
                .OrderBy(p => p.CreatedAt)
                .ToListAsync(ct);

            _logger.LogInformation("Retrieved {Count} Invertorys", Invertorys.Count);
            return Invertorys;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("GetAll operation was cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all Invertorys");
            throw;
        }
    }

    public async Task<Inventory> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid Invertory ID", nameof(id));

        try
        {
            ct.ThrowIfCancellationRequested();

            var Invertory = await _context.Invertories
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, ct);

            if (Invertory == null)
            {
                _logger.LogWarning("Invertory {InvertoryId} not found", id);
                throw new InvalidOperationException($"Invertory with ID {id} not found");
            }

            return Invertory;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("GetById operation was cancelled for Invertory {InvertoryId}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Invertory {InvertoryId}", id);
            throw;
        }
    }

    public async Task UpdateAsync(Inventory entity, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity.Id == Guid.Empty)
            throw new ArgumentException("Invalid Invertory ID", nameof(entity));

        try
        {
            ct.ThrowIfCancellationRequested();

            var existingInvertory = await _context.Invertories
                .FirstOrDefaultAsync(p => p.Id == entity.Id, ct);

            if (existingInvertory == null)
            {
                _logger.LogWarning("Invertory {InvertoryId} not found for update", entity.Id);
                throw new InvalidOperationException($"Invertory with ID {entity.Id} not found");
            }

            existingInvertory = entity.Adapt(existingInvertory);

            _context.Invertories.Update(existingInvertory);
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Invertory {InvertoryId} updated successfully", entity.Id);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Update operation was cancelled for Invertory {InvertoryId}", entity.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Invertory {InvertoryId}", entity.Id);
            throw;
        }
    }
}
