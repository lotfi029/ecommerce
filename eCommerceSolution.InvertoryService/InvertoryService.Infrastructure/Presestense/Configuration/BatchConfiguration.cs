using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryService.Infrastructure.Presestense.Configuration;

public class BatchConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {

        builder.ToTable("batches");
        
        builder.Property(x => x.InventoryId)
            .IsRequired()
            .HasColumnName("inventory_id");

        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasColumnName("quantity");

        builder.Property(x => x.ExpirationDate)
            .IsRequired()
            .HasColumnName("expiration_date");

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
    }
}
