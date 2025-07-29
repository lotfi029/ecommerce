using InventoryService.Core.IRepositories;
using InventoryService.Infrastructure.Presestense;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace InventoryService.Infrastructure.Presestense.Repository
{
    public class WarehouseRepository(
        ApplicationDbContext context,
        ILogger<WarehouseRepository> logger) : IWarehouseRepository
    {
        public async Task<Guid> AddAsync(Warehouse entity, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            try
            {
                await context.Warehouses.AddAsync(entity, ct);
                await context.SaveChangesAsync(ct);
                return entity.Id;
            }
            catch (OperationCanceledException)
            {
                logger.LogWarning("Operation was canceled while adding entity with ID: {Id}", entity.Id);
                return Guid.Empty;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding entity with ID: {Id}", entity.Id);
                throw new Exception("An error occurred while adding the entity.", ex);
            }
        }

        public async Task AddRangeAsync(IEnumerable<Warehouse> entities, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            try
            {
                await context.Warehouses.AddRangeAsync(entities, ct);
                await context.SaveChangesAsync(ct);
                logger.LogInformation("Entities added successfully");
            }
            catch (OperationCanceledException)
            {
                logger.LogWarning("Operation was canceled while adding entities.");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding entities.", ex);
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid Warehouse ID", nameof(id));

            try
            {
                await context.Warehouses
                    .Where(e => e.Id == id)
                    .ExecuteDeleteAsync(ct);
            }
            catch (OperationCanceledException)
            {
                logger.LogWarning("Operation was canceled while deleting entity with ID: {Id}", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting entity with ID: {Id}", id);
                throw new Exception("An error occurred while deleting the entity.", ex);
            }
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            try
            {
                return await context.Warehouses
                    .AsNoTracking()
                    .AnyAsync(e => e.Id == id, ct);
            }
            catch (OperationCanceledException)
            {
                logger.LogWarning("Operation was canceled while checking existence of entity with ID: {Id}", id);
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while checking existence of entity with ID: {Id}", id);
                throw new Exception("An error occurred while checking the entity's existence.", ex);
            }
        }
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
        public async Task<IEnumerable<Warehouse>> GetAllAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            try
            {
                return await context.Warehouses
                    .AsNoTracking()
                    .ToListAsync(ct);
            }
            catch (OperationCanceledException)
            {
                logger.LogWarning("Operation was canceled while retrieving all warehouses.");
                return [];
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving all warehouses.");
                throw new Exception("An error occurred while retrieving warehouses.", ex);
            }
        }

        public async Task<Warehouse?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid Warehouse ID", nameof(id));

            try
            {
                return await context.Warehouses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(w => w.Id == id, ct);
            }
            catch (OperationCanceledException)
            {
                logger.LogWarning("Operation was canceled while retrieving warehouse with ID: {Id}", id);
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving warehouse with ID: {Id}", id);
                throw new Exception("An error occurred while retrieving the warehouse.", ex);
            }
        }

        public async Task UpdateAsync(Warehouse entity, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            try
            {
                var existing = await context.Warehouses.FirstOrDefaultAsync(w => w.Id == entity.Id, ct);
                if (existing is null)
                {
                    logger.LogWarning("No warehouse found with ID: {Id}", entity.Id);
                    throw new KeyNotFoundException("Warehouse not found.");
                }

                context.Entry(existing).CurrentValues.SetValues(entity);
                await context.SaveChangesAsync(ct);
            }
            catch (OperationCanceledException)
            {
                logger.LogWarning("Operation was canceled while updating warehouse with ID: {Id}", entity.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating warehouse with ID: {Id}", entity.Id);
                throw new Exception("An error occurred while updating the warehouse.", ex);
            }
        }
    }
}