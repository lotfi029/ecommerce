namespace InventoryService.Core.Entities;

public class Reservation : BaseEntity, ISoftDeletable
{
    public Guid InventoryId { get; set; }
    public int Quantity { get; set; }
    public Guid? OrderId { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public DateTime? DeletedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public string? DeletedBy { get; private set; }


    public Inventory Inventory { get; set; } = null!;
}