using eCommerce.SharedKernal.Entities;

namespace InventoryService.Core.Entities;
public class Inventory : BaseEntity
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public int Reserved { get; set; }
}
