using eCommerce.SharedKernal.Entities;

namespace InventoryService.Core.Entities;

public class Reservation : BaseEntity
{
    public Guid InventoryId { get; set; }
    public int ReservedQuantity { get; set; }
    public Guid OrderId { get; set; }
    public string Status { get; set; } = string.Empty;
    public Inventory Inventory { get; set; } = null!;
}
