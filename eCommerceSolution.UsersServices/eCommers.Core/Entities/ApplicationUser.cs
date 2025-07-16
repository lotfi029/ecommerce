using System.Security.Principal;

namespace eCommers.Core.Entities;
public class ApplicationUser
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? Gender { get; set; }
    public ICollection<UserRoles> UserRoles { get; set; } = [];
}
