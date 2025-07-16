using eCommerce.Core.Abstractions;

namespace eCommerce.Core.Errors;
public class UserErrors
{
    public static Error UserNotFound
        => Error.NotFound($"Users.{nameof(UserNotFound)}", "the user with this id is not exit.");
}
