namespace InventoryService.Core.Errors;
public class InventoryErrors
{
    public static Error NotFound(Guid productId) =>
        Error.NotFound(
            "Inventory.NotFound",
            $"Inventory for product with ID '{productId}' was not found.");

    public static Error AlreadyExists(Guid productId) =>
        Error.Conflict(
            "Inventory.AlreadyExists",
            $"Inventory for product with ID '{productId}' already exists.");

    public static Error InvalidQuantity(int quantity) =>
        Error.BadRequest(
            "Inventory.InvalidQuantity",
            $"The provided quantity '{quantity}' is invalid. It must be greater than zero.");

    public static Error InvalidAccess(string userId) =>
        Error.Forbidden(
            "Inventory.InvalidAccess",
            $"User with ID '{userId}' does not have permission to access this inventory resource.");

    public static Error InsufficientStock(Guid productId, int requestedQuantity, int availableQuantity) =>
        Error.Conflict(
            "Inventory.InsufficientStock",
            $"Insufficient stock for product with ID '{productId}'. Requested quantity: {requestedQuantity}, Available quantity: {availableQuantity}.");

    public static Error InventoryUpdateFailed(Guid productId) =>
        Error.Conflict(
            "Inventory.InventoryUpdateFailed",
            $"Failed to update inventory for product with ID '{productId}'. Please try again later.");

    public static Error InventoryDeletionFailed(Guid productId) =>
        Error.Conflict(
            "Inventory.InventoryDeletionFailed",
            $"Failed to delete inventory for product with ID '{productId}'. Please try again later.");

    public static Error InventoryCreationFailed(Guid productId) =>
        Error.Conflict(
            "Inventory.InventoryCreationFailed",
            $"Failed to create inventory for product with ID '{productId}'. Please try again later.");
}
