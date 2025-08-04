namespace eCommerce.SharedKernal.Entities;
public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
    public string? DeletedBy { get; set; } = null;
}
