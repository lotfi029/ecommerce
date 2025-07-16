using eCommers.Core.Abstractions;
using eCommers.Core.Contracts.Users;
using eCommers.Core.Entities;
using eCommers.Core.RepositoryContracts;
using eCommers.Core.ServiceContracts;

namespace eCommers.Core.Services;

public class AuthService(IAuthRepository authRepository) : IAuthService
{
    public async Task<Result<AuthResponse>> GetTokenAsync(LoginRequest request, CancellationToken ct = default)
    {
        var result = await authRepository.GetTokenAsync(request.Email, request.Password);

        if (result.IsFailure)
            return result.Error;

        return result;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var user = new ApplicationUser
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password,
            Gender = request.Gender.ToString()
        };

        var result = await authRepository.AddUserAsync(user, ct);

        if (result.IsFailure)
            return result.Error;

        return new AuthResponse(
            result.Value.Id,
            result.Value.Name,
            result.Value.Email,
            result.Value.Gender,
            default!
            );
    }
}
