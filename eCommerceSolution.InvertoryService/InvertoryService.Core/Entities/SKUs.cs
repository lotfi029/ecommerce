using eCommerce.SharedKernal.Entities;

namespace InventoryService.Core.Entities;

public class SKUs : BaseEntity
{
    public Guid ProductId { get; set; }
    public string Attributes { get; set; } = string.Empty;   
}