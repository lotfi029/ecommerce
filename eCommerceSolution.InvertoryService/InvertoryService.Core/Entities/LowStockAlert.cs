using eCommerce.SharedKernal.Entities;

namespace InventoryService.Core.Entities;

public class LowStockAlert : BaseEntity
{
    public Guid InventoryId { get; set; }
    public int Threshold { get; set; }
    public bool AlertSent { get; set; }
    public Inventory Inventory { get; set; } = default!;
}