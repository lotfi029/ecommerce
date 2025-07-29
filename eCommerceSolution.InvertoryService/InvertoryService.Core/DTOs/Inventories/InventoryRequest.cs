namespace InventoryService.Core.DTOs.Inventories;
public record InventoryRequest(
    Guid ProductId,
    string SKU,
    Guid WarehouseId,
    int Quantity
    );
