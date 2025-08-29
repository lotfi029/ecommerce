using eCommerce.SharedKernal.Interfaces;
using System.Linq.Expressions;

namespace InventoryService.Core.IRepositories;

public interface IReservationRepository : IRepository<Reservation>
{
    Task<IEnumerable<Reservation>> GetAllWithFilters(Expression<Func<Reservation, bool>> expression, CancellationToken ct = default);
}
