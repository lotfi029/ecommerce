namespace InventoryService.Core.DTOs.LowStockAlerts;
public record LowStockAlertRequest(
    Guid ProductId,
    int Threshold,
    string SKU
    );