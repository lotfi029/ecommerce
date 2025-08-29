namespace InventoryService.Core.Errors;

public class ReservationErrors
{
    public static Error NotFound(Guid reservationId) =>
        Error.NotFound(
            "Reservation.NotFound",
            $"Reservation with ID '{reservationId}' was not found.");
    public static Error InvalidStatus(string status) =>
        Error.BadRequest(
            "Reservation.InvalidStatus",
            $"The provided reservation status '{status}' is invalid.");
    public static Error UpdateFailed(Guid reservationId) =>
        Error.Conflict(
            "Reservation.ReservationUpdateFailed",
            $"Failed to update reservation with ID '{reservationId}'. Please try again later.");
    public static Error CreationFailed(Guid inventoryId) =>
        Error.Conflict(
            "Reservation.ReservationCreationFailed",
            $"Failed to create reservation for inventory with ID '{inventoryId}'. Please try again later.");
    public static Error InsufficientStock(Guid inventoryId, int requestedQuantity, int availableQuantity) =>
        Error.Conflict(
            "Reservation.InsufficientStock",
            $"Insufficient stock for inventory with ID '{inventoryId}'. Requested quantity: {requestedQuantity}, Available quantity: {availableQuantity}.");
}