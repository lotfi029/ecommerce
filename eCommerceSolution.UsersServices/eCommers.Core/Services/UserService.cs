using eCommers.Core.Abstractions;
using eCommers.Core.Contracts.Users;
using eCommers.Core.RepositoryContracts;
using eCommers.Core.ServiceContracts;
using Mapster;

namespace eCommers.Core.Services;

public class UserService(IUserRepository _userRepository) : IUserService
{
    public async Task<Result<UserResponse>> GetUserByIdAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _userRepository.GetUserById(id, ct);

        if (user.IsFailure)
            return user.Error;

        var response = user.Value.Adapt<UserResponse>();

        return response;
    }
}
