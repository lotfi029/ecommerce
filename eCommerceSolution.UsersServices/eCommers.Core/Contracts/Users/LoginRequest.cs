namespace eCommers.Core.Contracts.Users;
public record LoginRequest(
    string Email,
    string Password
    );
