using eCommers.Core.Abstractions;
using eCommers.Core.Contracts.Users;
using eCommers.Core.Entities;

namespace eCommers.Core.RepositoryContracts;

public interface IAuthRepository
{
    Task<Result<ApplicationUser>> AddUserAsync(ApplicationUser user, CancellationToken ct);
    Task<Result<AuthResponse>> GetTokenAsync(string email, string password);
}