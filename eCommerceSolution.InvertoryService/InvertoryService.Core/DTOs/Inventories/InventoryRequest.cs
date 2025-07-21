namespace InventoryService.Core.DTOs.Inventories;
public record InventoryRequest(
    Guid ProductId,
    int Quantity
    );
