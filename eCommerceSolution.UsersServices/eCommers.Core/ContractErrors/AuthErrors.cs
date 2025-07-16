using eCommers.Core.Abstractions;

namespace eCommers.Core.ContractErrors;
public class AuthErrors
{
    public static Error DublicatedEmail 
        => Error.Conflict($"Auth.{nameof(DublicatedEmail)}", "this email is dublicated select another one.");
    public static Error AddUser
        => Error.BadRequest($"Auth.{nameof(AddUser)}", "Error ecure while adding new user");

    public static Error InvalidCredintial
        => Error.Unauthorized($"Auth.{nameof(InvalidCredintial)}", "invalid email or password.");
}
