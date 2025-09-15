namespace InventoryService.Core.DTOs.Reservations;
public record ReservationRequest(    
    int Quantity,
    Guid? OrderId
    );