namespace InventoryService.Core.Entities;

public class LowStockAlert : BaseEntity, ISoftDeletable
{
    public Guid InventoryId { get; set; }
    public int Threshold { get; set; }
    public bool AlertSent { get; set; }
    public Inventory Inventory { get; set; } = default!;

    public DateTime? DeletedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public string? DeletedBy { get; private set; }
}