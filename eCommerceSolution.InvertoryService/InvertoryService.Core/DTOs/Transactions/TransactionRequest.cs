namespace InventoryService.Core.DTOs.Transactions;
public record TransactionRequest(
    Guid ProductId,
    string SKU,
    int QuantityChanged,
    InventoryChangeType ChangeType,
    DateTime CreatedAt,
    Guid? OrderId
);
