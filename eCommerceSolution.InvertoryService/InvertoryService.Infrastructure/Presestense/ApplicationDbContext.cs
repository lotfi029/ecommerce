﻿using eCommerce.SharedKernal.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventoryService.Infrastructure.Presestense;
public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor httpContextAccessor) : DbContext(options)
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

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        var currentUserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)!.Value!;

        foreach (var entityTrack in entries)
        {
            if (entityTrack.State == EntityState.Added)
            {
                entityTrack.Entity.CreatedBy = currentUserId;
                entityTrack.Entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entityTrack.State == EntityState.Modified)
            {
                entityTrack.Entity.UpdatedBy = currentUserId;
                entityTrack.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
