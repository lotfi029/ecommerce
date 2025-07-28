namespace InventoryService.Core.Errors;

public class WarehouseErrors
{
    public static Error NotFound() =>
        Error.NotFound(
            "Warehouse.NotFound",
            "No warehouses were found.");
    public static Error NotFound(Guid warehouseId) =>
        Error.NotFound(
            "Warehouse.NotFound",
            $"Warehouse with ID '{warehouseId}' was not found.");
    public static Error AlreadyExists(Guid warehouseId) =>
        Error.Conflict(
            "Warehouse.AlreadyExists",
            $"Warehouse with ID '{warehouseId}' already exists.");
    public static Error InvalidAccess(string userId) =>
        Error.Forbidden(
            "Warehouse.InvalidAccess",
            $"User with ID '{userId}' does not have permission to access this warehouse resource.");
}