namespace InventoryService.Core.Errors;

public class TransactionErrors
{
    public static Error NotFound(Guid transactionId) =>
        Error.NotFound(
            "Transaction.NotFound",
            $"Transaction with ID '{transactionId}' was not found.");
    public static Error InvalidTransactionType(string transactionType) =>
        Error.BadRequest(
            "Transaction.InvalidType",
            $"The provided transaction type '{transactionType}' is invalid. Valid types are 'Addition' and 'Subtraction'.");
    public static Error InvalidQuantity(int quantity) =>
        Error.BadRequest(
            "Transaction.InvalidQuantity",
            $"The provided quantity '{quantity}' is invalid. It must be greater than zero.");
    public static Error InvalidAccess(string userId) =>
        Error.Forbidden(
            "Transaction.InvalidAccess",
            $"User with ID '{userId}' does not have permission to access this transaction resource.");
    public static Error TransactionCreationFailed(Guid inventoryId) =>
        Error.Conflict(
            "Transaction.CreationFailed",
            $"Failed to create transaction for inventory with ID '{inventoryId}'. Please try again later.");
    public static Error TransactionDeletionFailed(Guid transactionId) =>
        Error.Conflict(
            "Transaction.DeletionFailed",
            $"Failed to delete transaction with ID '{transactionId}'. Please try again later.");

    public static Error InsufficientInventory(Guid inventoryId, int availableQuantity, int requestedQuantity) =>
        Error.BadRequest(
            "Transaction.InsufficientInventory",
            $"Cannot process transaction for Inventory ID '{inventoryId}'. Available quantity is '{availableQuantity}', but requested quantity is '{requestedQuantity}'.");
}
