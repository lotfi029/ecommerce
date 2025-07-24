namespace InventoryService.Core.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public InventoryChangeType ChangeType { get; set; }
    public int QuantityChanged { get; set; }
    public DateTime Timestamp { get; set; }
    public string? UserId { get; set; }
    public string? OrderId { get; set; }
}