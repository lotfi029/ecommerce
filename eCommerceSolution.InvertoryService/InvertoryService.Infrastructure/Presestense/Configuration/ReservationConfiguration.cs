using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryService.Infrastructure.Presestense.Configuration;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("reservations");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Quantity)
            .IsRequired()
            .HasColumnName("quantity");

        builder.Property(e => e.Status)
            .IsRequired()
            .HasColumnName("status");

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

        builder.HasOne(e => e.Inventory)
            .WithMany(e => e.Reservations)
            .HasForeignKey(e => e.InventoryId)
            .IsRequired();

        builder.HasQueryFilter(p => !p.IsDeleted);

        builder.HasIndex(e => e.IsDeleted).IsUnique();
    }
}
