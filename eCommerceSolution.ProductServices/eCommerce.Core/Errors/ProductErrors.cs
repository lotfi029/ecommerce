namespace eCommerce.Core.Errors;
public class ProductErrors
{
    public static Error ProductNotFound
        => Error.NotFound($"Product.{nameof(ProductNotFound)}", "this product is not found");
    public static Error InvalidProductAccess
        => Error.Forbidden($"Product.{nameof(InvalidProductAccess)}", "you are not allowed to access this product");
    public static Error FailedOperation
        => Error.BadRequest($"Product.{nameof(FailedOperation)}", "failed operation");
    public static Error ProductImageNotFound
        => Error.NotFound($"Product.{nameof(ProductImageNotFound)}", "this product image is not found");
    public static Error InvalidProductImage
        => Error.BadRequest($"Product.{nameof(InvalidProductImage)}", "this product image is invalid");
}
