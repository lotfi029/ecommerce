using eCommers.Core.Abstractions;

namespace eCommers.Core.ContractErrors;

public class UserErrors
{
    public static Error UserNotFound
        => Error.NotFound($"User.{nameof(UserNotFound)}", "user not found");
}