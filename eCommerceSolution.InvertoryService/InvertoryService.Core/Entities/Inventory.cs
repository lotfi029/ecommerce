using eCommerce.SharedKernal.Entities;

namespace InventoryService.Core.Entities;
public class Inventory : BaseEntity
{
    public Guid ProductId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Reserved { get; set; }
    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;


    public static Inventory Create(Guid productId, int quantity, string createdBy)
    {
        return new()
        {
            ProductId = productId,
            Quantity = quantity,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Inventory UpdateQuantity(Inventory inventory, int quantity)
    {
        inventory.Quantity += quantity;
        inventory.UpdatedAt = DateTime.UtcNow;
        inventory.UpdatedBy = inventory.CreatedBy;
        
        return inventory;
    }
}
