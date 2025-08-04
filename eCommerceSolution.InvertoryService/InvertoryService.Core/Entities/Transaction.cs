using eCommerce.SharedKernal.Entities;

namespace InventoryService.Core.Entities;

public class Transaction : BaseEntity
{
    public Guid InventoryId { get; set; }
    public InventoryChangeType ChangeType { get; set; }
    public int QuantityChanged { get; set; }
    public Guid? OrderId { get; set; }
    public Inventory Inventory { get; set; } = default!;
}