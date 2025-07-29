namespace InventoryService.Core.Errors;

public class LowStockAlertErrors
{
    public static Error NotFound(Guid productId) =>
        Error.NotFound(
            "LowStockAlert.NotFound",
            $"Low stock alert for product with ID '{productId}' was not found.");

    public static Error NotFound() => 
        Error.NotFound(
            "LowStockAlert.NotFound",
            "Low stock alert was not found.");
    public static Error AlreadyExists(Guid productId, string sku) =>
        Error.Conflict(
            "LowStockAlert.AlreadyExists",
            $"Low stock alert for product with ID '{productId}' and SKU '{sku}' already exists.");

    public static Error CreationFailed(Guid productId, string sku) =>
        Error.Conflict(
            "LowStockAlert.CreationFailed",
            $"Failed to create low stock alert for product with ID '{productId}' and SKU '{sku}'. Please try again later.");
    public static Error DeletionFailed(Guid productId, string sku) =>
        Error.Conflict(
            "LowStockAlert.DeletionFailed",
            $"Failed to delete low stock alert for product with ID '{productId}' and SKU '{sku}'. Please try again later.");
    public static Error InvalidSKU(string sku) =>
        Error.BadRequest(
            "LowStockAlert.InvalidSKU",
            $"The provided SKU '{sku}' is invalid.");
    public static Error InvalidAccess(string userId) =>
        Error.Forbidden(
            "LowStockAlert.InvalidAccess",
            $"User with ID '{userId}' does not have permission to access this low stock alert resource.");
}
