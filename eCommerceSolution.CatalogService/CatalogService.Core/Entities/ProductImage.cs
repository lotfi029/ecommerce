using Microsoft.EntityFrameworkCore;

namespace CatalogService.Core.Entities;
[Owned]
public class ProductImage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}