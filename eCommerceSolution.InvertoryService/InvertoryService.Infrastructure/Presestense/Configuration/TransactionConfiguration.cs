using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryService.Infrastructure.Presestense.Configuration;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasColumnName("product_id");

        builder.Property(x => x.SKU)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("sku");

        builder.Property(x => x.QuantityChanged)
            .IsRequired()
            .HasColumnName("quantity_changed");

        builder.Property(x => x.Timestamp)
            .IsRequired()
            .HasColumnName("timestamp");

        builder.Property(x => x.UserId)
            .IsRequired(false)
            .HasMaxLength(450)
            .HasColumnName("user_id");

        builder.Property(x => x.OrderId)
            .IsRequired(false)
            .HasMaxLength(450)
            .HasColumnName("order_id");
    }
}