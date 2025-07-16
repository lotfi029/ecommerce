using Microsoft.EntityFrameworkCore;

namespace eCommerceCatalogService.Core.Entities;
[Owned]
public class ProductImage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}