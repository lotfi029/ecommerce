﻿using eCommerceCatalogService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerceCatalogService.Infrastructure.Persistense.Configurations;

public class CatalogProductConfiguration : IEntityTypeConfiguration<CatalogProduct>
{
    public void Configure(EntityTypeBuilder<CatalogProduct> builder)
    {
        builder.OwnsMany(e => e.ProductImages)
            .ToTable("images")
            .WithOwner()
            .HasForeignKey("ProductId");
    }
}
