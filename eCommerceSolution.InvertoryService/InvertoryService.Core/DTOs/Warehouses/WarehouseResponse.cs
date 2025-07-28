namespace InventoryService.Core.DTOs.Warehouses;
public record WarehouseResponse(
    Guid Id,
    string Name,
    string Location
    );