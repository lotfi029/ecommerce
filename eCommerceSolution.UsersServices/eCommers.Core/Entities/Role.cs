namespace eCommers.Core.Entities;

public class Role
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string? Name { get; set; }
    public ICollection<UserRoles> UserRoles { get; set; } = [];
}
