﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryService.Infrastructure.Presestense.Configuration;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("warehouses");
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        builder.Property(x => x.Location)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("location");

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
