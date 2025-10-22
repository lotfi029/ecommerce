namespace InventoryService.Core.DTOs.Transactions;
public record TransactionRequest(
    int QuantityChanged,
    string Reason
//Guid? OrderId
);
