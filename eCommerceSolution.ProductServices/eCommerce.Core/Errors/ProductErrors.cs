namespace eCommerce.Core.Errors;
public class ProductErrors
{
    public static Error NotFound
        => Error.NotFound($"Product.{nameof(NotFound)}", "this product is not found");
    public static Error InvalidAccess
        => Error.Forbidden($"Product.{nameof(InvalidAccess)}", "you are not allowed to access this product");
    public static Error FailedOperation
        => Error.BadRequest($"Product.{nameof(FailedOperation)}", "failed operation");
    public static Error ImageNotFound
        => Error.NotFound($"Product.{nameof(ImageNotFound)}", "this product image is not found");
    public static Error InvalidImage
        => Error.BadRequest($"Product.{nameof(InvalidImage)}", "this product image is invalid");
}
