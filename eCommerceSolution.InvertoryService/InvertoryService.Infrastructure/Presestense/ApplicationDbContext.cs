using eCommerce.SharedKernal.Entities;
using eCommerce.SharedKernal.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace InventoryService.Infrastructure.Presestense;
public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor httpContextAccessor) : DbContext(options)
{
    public DbSet<Inventory> Invertories { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var updateEntries = ChangeTracker.Entries<BaseEntity>();
        var currentUserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)!.Value!;

        foreach (var entityTrack in updateEntries)
        {
            switch (entityTrack.State)
            {
                case EntityState.Added:
                    entityTrack.Entity.CreatedBy = currentUserId;
                    entityTrack.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entityTrack.Entity.UpdatedBy = currentUserId;
                    entityTrack.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                default:
                    break;
            }
        }

        var deletedEntries = ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(x => x.State == EntityState.Deleted);
        
        foreach (var entityTrack in deletedEntries)
        {
            entityTrack.State = EntityState.Modified;
            entityTrack.Property(nameof(ISoftDeletable.IsDeleted)).CurrentValue = true;
            entityTrack.Property(nameof(ISoftDeletable.DeletedAt)).CurrentValue = DateTime.UtcNow;
            entityTrack.Property(nameof(ISoftDeletable.DeletedBy)).CurrentValue = currentUserId;
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
