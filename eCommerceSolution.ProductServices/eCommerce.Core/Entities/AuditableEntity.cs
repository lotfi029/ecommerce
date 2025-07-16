namespace eCommerce.Core.Entities;

public class AuditableEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string CreatedBy { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime? UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
