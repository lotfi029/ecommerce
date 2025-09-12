namespace InventoryService.Core.DTOs.Transactions;
public record TransactionRequest(
    Guid InventoryId,
    int QuantityChanged,
    InventoryChangeType ChangeType,
    DateTime CreatedAt,
    Guid? OrderId
);
