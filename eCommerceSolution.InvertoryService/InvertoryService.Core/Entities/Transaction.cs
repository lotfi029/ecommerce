namespace InventoryService.Core.Entities;

public class Transaction : BaseEntity, ISoftDeletable
{
    public Guid InventoryId { get; set; }
    public InventoryChangeType ChangeType { get; set; } 
    public string Reason { get; set; } = string.Empty;
    public int QuantityChanged { get; set; }
    public Guid? OrderId { get; set; }
    public Inventory Inventory { get; set; } = default!;

    public DateTime? DeletedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public string? DeletedBy { get; private set; }
}