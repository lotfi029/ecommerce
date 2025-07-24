using eCommerce.SharedKernal.Entities;

namespace InventoryService.Core.Entities;

public class Batch : BaseEntity
{
    public Guid ProductId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime ExpirationDate { get; set; }


    public Batch() { }
    public Batch(Guid productId, string sku, int quantity, DateTime expirationDate)
    {
        ProductId = productId;
        SKU = sku;
        Quantity = quantity;
        ExpirationDate = expirationDate;
    }
}