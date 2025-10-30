namespace CatalogService.Core.Entities;

public class CatalogProduct : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public Guid? CatagoryId { get; set; }
    public bool IsActive { get; set; }
    public ICollection<ProductImage>? ProductImages { get; set; } = [];


    public void UpdateAuditable(string? updatedBy)
    {
        UpdatedAt = DateTime.UtcNow;

        if (updatedBy is not null)
            UpdatedBy = updatedBy;
    }
}
