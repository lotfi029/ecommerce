namespace InventoryService.Core.DTOs.LowStockAlerts;
public record LowStockRequest(
    Guid ProductId,
    int Threshold,
    string SKU
    );
