using eCommerce.SharedKernal.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryService.Core.Entities;
public class Inventory : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Reserved { get; set; }
    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;
    [NotMapped]
    public int Available => Quantity - Reserved;
    public static Inventory Create(Guid productId, string sku, int quantity, Guid warehouse, string createdBy)
    {
        return new()
        {
            ProductId = productId,
            Quantity = quantity,
            SKU = sku,
            CreatedBy = createdBy,
            Reserved = 0,
            CreatedAt = DateTime.UtcNow,
            WarehouseId = warehouse
        };
    }
    public void Reserve(int quantity)
    {
        if (quantity > Available)
            throw new InvalidOperationException("Insufficient stock to reserve.");

        Reserved += quantity;
    }
    public void Release(int quantity)
    {
        Reserved = Math.Max(0, Reserved - quantity);
    }
    public void Restock(int quantity)
    {
        Quantity += quantity;
    }
    public void Deduct(int quantity)
    {
        if (quantity > Available)
            throw new InvalidOperationException("Insufficient stock to deduct.");

        Quantity -= quantity;
    }
}
