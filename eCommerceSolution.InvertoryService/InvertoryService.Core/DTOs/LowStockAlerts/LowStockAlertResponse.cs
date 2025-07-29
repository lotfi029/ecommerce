namespace InventoryService.Core.DTOs.LowStockAlerts;

public record LowStockAlertResponse(
    Guid Id,
    Guid ProductId,
    int Threshold,
    string SKU,
    bool AlertSent
    );