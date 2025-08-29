namespace eCommerce.SharedKernal.Interfaces;
public interface ISoftDeletable
{
    public DateTime? DeletedAt { get; }
    public bool IsDeleted { get; }
    public string? DeletedBy { get; }
}
