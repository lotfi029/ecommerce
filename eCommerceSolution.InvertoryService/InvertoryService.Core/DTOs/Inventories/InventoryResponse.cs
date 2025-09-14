namespace InventoryService.Core.DTOs.Inventories;

public record InventoryResponse(
    Guid Id,
    string SKU,
    Guid ProductId,
    int Quantity,
    int ReorderLevel,
    Guid WarehouseId,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );