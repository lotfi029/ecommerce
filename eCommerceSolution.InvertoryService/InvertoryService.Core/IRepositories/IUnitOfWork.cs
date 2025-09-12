using Microsoft.EntityFrameworkCore.Storage;

namespace InventoryService.Core.IRepositories;
public interface IUnitOfWork : IDisposable
{
    IInventoryRepository InventoryRepository { get; }
    IReservationRepository ReservationRepository { get; }
    IWarehouseRepository WarehouseRepository { get; }
    ITransactionRepository TransactionRepository { get; }

    Task<int> CommitChangesAsync(CancellationToken ct = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);
}
