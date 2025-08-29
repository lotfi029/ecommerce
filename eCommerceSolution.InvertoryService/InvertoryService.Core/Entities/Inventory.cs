namespace InventoryService.Core.Entities;

public class Inventory : BaseEntity, ISoftDeletable
{
    public Guid ProductId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
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
        Guid warehouse,
        string createdBy
        ) : this()
    {
        ProductId = productId;
        SKU = sku;
        Quantity = quantity;
        CreatedBy = createdBy;
        //Reserved = 0;
        CreatedAt = DateTime.UtcNow;
        WarehouseId = warehouse;

    }
    public Inventory()
    {
        
    }
    //public void Reserve(int quantity)
    //{
    //    if (quantity > Available)
    //        throw new InvalidOperationException("Insufficient stock to reserve.");

    //    Reserved += quantity;
    //}
    //public void Release(int quantity)
    //{
    //    Reserved = Math.Max(0, Reserved - quantity);
    //}
    public void Restock(int quantity)
    {
        Quantity += quantity;
    }
    //public void Deduct(int quantity)
    //{
    //    if (quantity > Available)
    //        throw new InvalidOperationException("Insufficient stock to deduct.");

    //    Quantity -= quantity;
    //}
}
