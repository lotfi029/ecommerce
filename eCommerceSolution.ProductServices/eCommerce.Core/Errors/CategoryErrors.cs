namespace eCommerce.Core.Errors;

public class CategoryErrors
{
    public static Error CategoryNotFound
        => Error.NotFound($"Category.{nameof(CategoryNotFound)}", "this category is not found");
    public static Error InvalidCategoryAccess
        => Error.Forbidden($"Category.{nameof(InvalidCategoryAccess)}", "you are not allowed to access this category");
    public static Error FailedOperation
        => Error.BadRequest($"Category.{nameof(FailedOperation)}", "failed operation");
    public static Error CategoryImageNotFound
        => Error.NotFound($"Category.{nameof(CategoryImageNotFound)}", "this category image is not found");
    public static Error InvalidCategoryImage
        => Error.BadRequest($"Category.{nameof(InvalidCategoryImage)}", "this category image is invalid");
}