using eCommerce.SharedKernal.Interfaces;
using System.Linq.Expressions;

namespace InventoryService.Core.IRepositories;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetAllWithFilters(Expression<Func<Transaction, bool>> filter, CancellationToken ct = default);
    Task<IEnumerable<Transaction>> GetByProductIdAsync(Guid productId, CancellationToken ct = default);
    Task<IEnumerable<Transaction>> GetBySKUAsync(string sku, CancellationToken ct = default);
}