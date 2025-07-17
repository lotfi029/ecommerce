using InventoryService.Core.Entities;
using Microsoft.EntityFrameworkCore;
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

        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasColumnName("quantity");
        
        builder.Property(x => x.Reserved)
            .IsRequired()
            .HasColumnName("reserved");

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
