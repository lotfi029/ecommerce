using eCommers.Core.Abstractions;
using eCommers.Core.Contracts.Users;

namespace eCommers.Core.ServiceContracts;
public interface IUserService
{
    Task<Result<UserResponse>> GetUserByIdAsync(Guid id, CancellationToken ct = default);
}
