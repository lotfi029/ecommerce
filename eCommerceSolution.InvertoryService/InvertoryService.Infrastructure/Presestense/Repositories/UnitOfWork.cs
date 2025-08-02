using Microsoft.EntityFrameworkCore.Storage;

namespace InventoryService.Infrastructure.Presestense.Repositories;
public class UnitOfWork(
    ApplicationDbContext context,
    ILoggerFactory loggerFactory) : IUnitOfWork
{
    private readonly ApplicationDbContext _context
    = context ?? throw new ArgumentNullException(nameof(ApplicationDbContext));
    private readonly ILoggerFactory _loggerFactory
        = loggerFactory ?? throw new ArgumentNullException(nameof(ILoggerFactory));

    private bool _disposed = false;
    public IInventoryRepository InventoryRepository => 
        new InventoryRepository(_context, _loggerFactory.CreateLogger<Repository<Inventory>>(), _loggerFactory.CreateLogger<InventoryRepository>());

    public ILowStockAlertRepository LowStockAlertRepository =>
        new LowStockAlertRepository(_context, _loggerFactory.CreateLogger<Repository<LowStockAlert>>(), _loggerFactory.CreateLogger<LowStockAlertRepository>());

    public IWarehouseRepository WarehouseRepository => 
        new WarehouseRepository(_context, _loggerFactory.CreateLogger<Repository<Warehouse>>(), _loggerFactory.CreateLogger<WarehouseRepository>());

    public ITransactionRepository TransactionRepository =>
        new TransactionRepository(_context, _loggerFactory.CreateLogger<Repository<Transaction>>(), _loggerFactory.CreateLogger<TransactionRepository>());

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => await _context.Database.BeginTransactionAsync(cancellationToken);
    

    public async Task<int> CommitChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
    

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        try
        {
            await CommitChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(transaction, cancellationToken);
            throw;
        }
    }
    public async Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        await transaction.RollbackAsync(cancellationToken);
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }

}
