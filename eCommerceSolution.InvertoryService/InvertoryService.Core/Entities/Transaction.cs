using eCommerce.SharedKernal.Entities;

namespace InventoryService.Core.Entities;

public class Transaction : BaseEntity
{
    public Guid ProductId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public InventoryChangeType ChangeType { get; set; }
    public int QuantityChanged { get; set; }
    public Guid? OrderId { get; set; }
}