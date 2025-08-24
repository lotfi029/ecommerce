using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryService.Infrastructure.Presestense.Configuration;
public class InvertoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable("invertories");

        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasColumnName("product_id");
        
        builder.Property(x => x.SKU)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("sku");

        builder.Property(x => x.WarehouseId)
            .IsRequired()
            .HasColumnName("warehouse_id");

        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasColumnName("quantity");
        
        builder.HasOne(x => x.Warehouse)
            .WithMany(e => e.Inventories)
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasIndex(e => new { e.ProductId, e.SKU })
            .IsUnique()
            .HasDatabaseName("IX_Inventories_ProductId_SKU");

        // audit properties configuration
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasColumnName("created_by");

        builder.Property(e => e.UpdatedAt)
            .IsRequired(false)
            .HasColumnName("updated_at");

        builder.Property(e => e.UpdatedBy)
            .IsRequired(false)
            .HasColumnName("updated_by");

        builder.Property(e => e.IsDeleted)
            .IsRequired()
            .HasColumnName("is_deleted");

        builder.Property(e => e.DeletedAt)
            .IsRequired(false)
            .HasColumnName("deleted_at");

        builder.Property(e => e.DeletedBy)
            .IsRequired(false)
            .HasColumnName("deleted_by");
    }
}
