namespace InventoryService.Core.DTOs;
public record ReservationResponse(
    Guid Id,
    Guid InventoryId,
    int Quantity
    );