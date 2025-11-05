namespace eCommerce.Core.Errors;

public class CategoryErrors
{
    public static Error NotFound
        => Error.NotFound($"Category.{nameof(NotFound)}", "this category is not found");
    public static Error InvalidAccess
        => Error.Forbidden($"Category.{nameof(InvalidAccess)}", "you are not allowed to access this category");
    public static Error FailedOperation
        => Error.BadRequest($"Category.{nameof(FailedOperation)}", "failed operation");
    public static Error ImageNotFound
        => Error.NotFound($"Category.{nameof(ImageNotFound)}", "this category image is not found");
    public static Error InvalidImage
        => Error.BadRequest($"Category.{nameof(InvalidImage)}", "this category image is invalid");
}