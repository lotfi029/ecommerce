namespace InventoryService.Core.Entities;

public class Warehouse : BaseEntity, ISoftDeletable
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime? DeletedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public string? DeletedBy { get; private set; }
    public List<Inventory> Inventories { get; set; } = [];
    public Warehouse() { }
    public Warehouse(string name, string location)
    {
        Name = name;
        Location = location;
    }
}