using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryService.Infrastructure.Presestense.Configuration;

public class LowStockAlertConfiguration : IEntityTypeConfiguration<LowStockAlert>
{
    public void Configure(EntityTypeBuilder<LowStockAlert> builder)
    {
        builder.ToTable("low_stock_alerts");
        
        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasColumnName("product_id");
        
        builder.Property(x => x.SKU)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("sku");

        builder.Property(x => x.Threshold)
            .IsRequired()
            .HasColumnName("threshold");

        builder.Property(x => x.AlertSent)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("alert_sent");

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
    }
}