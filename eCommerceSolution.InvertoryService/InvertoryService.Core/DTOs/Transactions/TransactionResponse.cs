namespace InventoryService.Core.DTOs.Transactions;

public record TransactionResponse(
    Guid Id,
    Guid InventoryId,
    int QuantityChanged,
    string ChangeType,
    DateTime CreatedAt
    //Guid? OrderId
);
