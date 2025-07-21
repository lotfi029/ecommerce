using eCommerce.SharedKernal.Entities;

namespace InventoryService.Core.Entities;

public class LowStockAlert : BaseEntity
{
    public Guid ProductId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public int Threshold { get; set; }
    public bool AlertSent { get; set; }
}