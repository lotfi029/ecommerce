using eCommers.Core.Abstractions;
using eCommers.Core.Contracts.Users;

namespace eCommers.Core.ServiceContracts;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(LoginRequest request, CancellationToken ct = default);
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
}