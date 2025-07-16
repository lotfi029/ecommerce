namespace eCommers.Core.Entities;

public class UserRoles
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public ApplicationUser User { get; set; } = default!;
    public Role Role { get; set; } = default!;
}