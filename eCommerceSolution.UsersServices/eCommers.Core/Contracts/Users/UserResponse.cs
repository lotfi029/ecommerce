namespace eCommers.Core.Contracts.Users;
public record UserResponse(
    Guid Id,
    string Name,
    string Email,
    string Gender
    );
