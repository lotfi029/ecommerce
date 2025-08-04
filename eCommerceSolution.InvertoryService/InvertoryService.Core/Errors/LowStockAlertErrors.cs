namespace InventoryService.Core.Errors;

public class LowStockAlertErrors
{
    public static Error NotFound(Guid inventoryId) =>
        Error.NotFound(
            "LowStockAlert.NotFound",
            $"Low stock alert for inventory with ID '{inventoryId}' was not found.");

    public static Error NotFound() => 
        Error.NotFound(
            "LowStockAlert.NotFound",
            "Low stock alert was not found.");
    public static Error AlreadyExists(Guid inventoryId) =>
        Error.Conflict(
            "LowStockAlert.AlreadyExists",
            $"Low stock alert for inventory with ID '{inventoryId}' already exists.");

    public static Error CreationFailed(Guid inventoryId) =>
        Error.Conflict(
            "LowStockAlert.CreationFailed",
            $"Failed to create low stock alert for inventory with ID '{inventoryId}'. Please try again later.");
    public static Error DeletionFailed(Guid inventoryId) =>
        Error.Conflict(
            "LowStockAlert.DeletionFailed",
            $"Failed to delete low stock alert for inventory with ID '{inventoryId}'. Please try again later.");
    public static Error InvalidAccess(string userId) =>
        Error.Forbidden(
            "LowStockAlert.InvalidAccess",
            $"User with ID '{userId}' does not have permission to access this low stock alert resource.");
}
