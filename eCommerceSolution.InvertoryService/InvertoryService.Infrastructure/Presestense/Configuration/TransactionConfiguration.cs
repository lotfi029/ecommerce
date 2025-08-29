using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryService.Infrastructure.Presestense.Configuration;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        builder.Property(x => x.InventoryId)
            .IsRequired()
            .HasColumnName("inventory_id");

        builder.Property(x => x.QuantityChanged)
            .IsRequired()
            .HasColumnName("quantity_changed");

        builder.Property(x => x.OrderId)
            .IsRequired(false)
            .HasMaxLength(450)
            .HasColumnName("order_id");

        builder.HasOne(x => x.Inventory)
            .WithMany(e => e.Transactions)
            .HasForeignKey(x => x.InventoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

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

        builder.HasQueryFilter(p => !p.IsDeleted);

        builder.HasIndex(e => e.IsDeleted).HasFilter("is_deleted = 0");
    }
}