namespace eCommers.Core.Contracts.Users;

public record RegisterRequest(
    string? Name,
    string? Email,
    GenderOptions? Gender,
    string? Password
    );
