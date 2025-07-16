using Microsoft.AspNetCore.Authentication.BearerToken;

namespace eCommers.Core.Contracts.Users;

public record AuthResponse(
    Guid Id,
    string? Name,
    string? Email,
    string? Gender,
    AccessTokenResponse Token
)
{
    public AuthResponse() : this(Guid.Empty, null, null, null, default!)
    {
        
    }
}
