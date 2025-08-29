using System.Linq.Expressions;

namespace InventoryService.Infrastructure.Presestense.Repositories;

public class ReservationRepository(
    ApplicationDbContext context,
    ILogger<Repository<Reservation>> repositoryLogger,
    ILogger<ReservationRepository> logger
    ) : Repository<Reservation>(context, repositoryLogger), IReservationRepository
{
    public async Task<IEnumerable<Reservation>> GetAllWithFilters(Expression<Func<Reservation, bool>> expression, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(expression, nameof(expression));

        if (_context.Reservations is null)
        {
            logger.LogError("Reservations DbSet is null");
            throw new InvalidOperationException("Reservations DbSet is not initialized");
        }
        var response = await _context.Reservations
            .AsNoTracking()
            .Where(expression)
            .ToListAsync(ct);

        return response;
    }
}
