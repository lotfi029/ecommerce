namespace InventoryService.Core.Entities;

public class Inventory : BaseEntity, ISoftDeletable
{
    public Guid ProductId { get; set; } // fixed after creation
    public string SKU { get; set; } = string.Empty; // fixed after creation
    public int Quantity { get; set; } // will be updated by another entities (transaction and reservation)
    public int ReorderLevel { get; set; } // low stock alert threshold 
    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;
    public ICollection<Reservation> Reservations { get; set; } = [];
    public ICollection<Transaction> Transactions { get; set; } = [];

    public DateTime? DeletedAt { get; private set; }

    public bool IsDeleted { get; private set; }

    public string? DeletedBy { get; private set; } 

    public Inventory(
        Guid productId,
        string sku,
        int quantity,
        int reorderLevel,
        Guid warehouseId
        ) : this()
    {
        ProductId = productId;
        SKU = sku;
        Quantity = quantity;
        CreatedAt = DateTime.UtcNow;
        WarehouseId = warehouseId;
        ReorderLevel = reorderLevel;

    }
    public Inventory()
    {
        
    }
}
