using eCommerce.SharedKernal.Interfaces;
using InventoryService.Core.DTOs.Inventories;
using System.Linq.Expressions;

namespace InventoryService.Core.IRepositories;
public interface IInventoryRepository : IRepository<Inventory>
{
    Task<Inventory> GetAllWithFilter(Expression<Func<Inventory, bool>> expression, CancellationToken ct = default);

}