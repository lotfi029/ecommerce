using eCommerce.SharedKernal.Interfaces;
using System.Linq.Expressions;

namespace InventoryService.Core.IRepositories;
public interface IInventoryRepository : IRepository<Inventory>
{
    Task<IEnumerable<Inventory>> GetAllWithFilter(Expression<Func<Inventory, bool>> expression, CancellationToken ct = default);

}