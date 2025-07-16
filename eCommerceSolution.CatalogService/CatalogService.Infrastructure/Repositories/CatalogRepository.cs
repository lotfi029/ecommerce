using CatalogService.Infrastructure.Persistense;
using eCommerceCatalogService.Core.Entities;
using eCommerceCatalogService.Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace eCommerceCatalogService.Infrastructure.Repositories;

public class CatalogRepository(ApplicationDbContext context, ILogger<CatalogRepository> logger) : ICatalogRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ILogger<CatalogRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Guid> AddAsync(CatalogProduct entity, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            ct.ThrowIfCancellationRequested();

            await _context.CatalogProducts.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Product {ProductId} added successfully", entity.Id);
            return entity.Id;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Add operation was cancelled for product {ProductName}", entity.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding product {ProductName}", entity.Name);
            throw;
        }
    }
    public async Task AddRangeAsync(IEnumerable<CatalogProduct> entity, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            ct.ThrowIfCancellationRequested();

            await _context.CatalogProducts.AddRangeAsync(entity, ct);
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Product {ProductIds} added successfully", entity.Select(e => e.Id));
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Add operation was cancelled for product {Productid}", entity.Select(e => e.Id));
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding product {Productid}", entity.Select(e => e.Id));
            throw;
        }
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid product ID", nameof(id));

        try
        {
            ct.ThrowIfCancellationRequested();

            var rowsDeleted = await _context.CatalogProducts
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync(ct);
            
            _logger.LogInformation("Product {ProductId} deleted successfully", id);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Delete operation was cancelled for product {ProductId}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product {ProductId}", id);
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

            return await _context.CatalogProducts
                .AnyAsync(p => p.Id == id, ct);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Exists check was cancelled for product {ProductId}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if product {ProductId} exists", id);
            throw;
        }
    }

    public async Task<IEnumerable<CatalogProduct>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var products = await _context.CatalogProducts
                .AsNoTracking()
                .OrderBy(p => p.CreatedAt)
                .ToListAsync(ct);

            _logger.LogInformation("Retrieved {Count} products", products.Count);
            return products;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("GetAll operation was cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all products");
            throw;
        }
    }

    public async Task<CatalogProduct> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid product ID", nameof(id));

        try
        {
            ct.ThrowIfCancellationRequested();

            var product = await _context.CatalogProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, ct);

            if (product == null)
            {
                _logger.LogWarning("Product {ProductId} not found", id);
                throw new InvalidOperationException($"Product with ID {id} not found");
            }

            return product;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("GetById operation was cancelled for product {ProductId}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product {ProductId}", id);
            throw;
        }
    }

    public async Task UpdateAsync(CatalogProduct entity, CancellationToken ct = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        if (entity.Id == Guid.Empty)
            throw new ArgumentException("Invalid product ID", nameof(entity));

        try
        {
            ct.ThrowIfCancellationRequested();

            var existingProduct = await _context.CatalogProducts
                .FirstOrDefaultAsync(p => p.Id == entity.Id, ct);

            if (existingProduct == null)
            {
                _logger.LogWarning("Product {ProductId} not found for update", entity.Id);
                throw new InvalidOperationException($"Product with ID {entity.Id} not found");
            }

            // Update properties
            existingProduct.Name = entity.Name;
            existingProduct.Description = entity.Description;
            existingProduct.Price = entity.Price;
            existingProduct.CatagoryId = entity.CatagoryId;
            existingProduct.IsActive = entity.IsActive;
            entity.UpdateAuditable(null);

            _context.CatalogProducts.Update(existingProduct);
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Product {ProductId} updated successfully", entity.Id);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Update operation was cancelled for product {ProductId}", entity.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product {ProductId}", entity.Id);
            throw;
        }
    }
}