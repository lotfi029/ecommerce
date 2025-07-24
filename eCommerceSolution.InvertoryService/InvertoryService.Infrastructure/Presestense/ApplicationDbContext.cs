using InventoryService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Presestense;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Inventory> Invertories { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<LowStockAlert> LowStockAlerts { get; set; }
    public DbSet<Batch> Batches { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
