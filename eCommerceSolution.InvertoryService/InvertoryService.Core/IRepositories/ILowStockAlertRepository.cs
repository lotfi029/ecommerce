using eCommerce.SharedKernal.Interfaces;
using System.Linq.Expressions;

namespace InventoryService.Core.IRepositories;

public interface ILowStockAlertRepository : IRepository<LowStockAlert>
{
    Task<IEnumerable<LowStockAlert>> GetAllWithFilters(Expression<Func<LowStockAlert, bool>> filer, CancellationToken ct = default);
}