using eCommerce.SharedKernal.Interfaces;
using System.Linq.Expressions;

namespace InventoryService.Core.IRepositories;
public interface IWarehouseRepository : IRepository<Warehouse>
{
    Task<IEnumerable<Warehouse>> GetAllWithFilters(Expression<Func<Warehouse, bool>> filer, CancellationToken ct = default);
}
