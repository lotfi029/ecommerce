using eCommerce.SharedKernal.Entities;

namespace InventoryService.Core.Entities;

public class Batch : BaseEntity
{
    public Guid InventoryId { get; set; } 
    public int Quantity { get; set; }
    public DateTime ExpirationDate { get; set; }
    public Inventory Inventory { get; set; } = default!;

    public Batch() { }
    public Batch(Guid inventoryId, int quantity, DateTime expirationDate)
    {
        InventoryId = inventoryId;
        Quantity = quantity;
        ExpirationDate = expirationDate;
    }
}