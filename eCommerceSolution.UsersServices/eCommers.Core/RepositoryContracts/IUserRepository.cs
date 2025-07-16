using eCommers.Core.Abstractions;
using eCommers.Core.Entities;

namespace eCommers.Core.RepositoryContracts;
public interface IUserRepository
{
    Task<Result<ApplicationUser>> GetUserById(Guid id, CancellationToken ct = default);
}
