namespace InventoryService.Core.DTOs.Reservations;
public record ReservationRequest(
    Guid InventoryId,
    int ReservationQuantity,
    Guid? OrderId
    );