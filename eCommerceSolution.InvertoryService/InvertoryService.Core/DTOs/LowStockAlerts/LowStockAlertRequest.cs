namespace InventoryService.Core.DTOs.LowStockAlerts;
public record LowStockAlertRequest(
    Guid InventoryId,
    int Threshold
    );