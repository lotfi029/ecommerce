namespace eCommerce.Core.Contracts;
public record UserDTO(
    Guid Id,
    string Email,
    string Name,
    string Gender
    );
