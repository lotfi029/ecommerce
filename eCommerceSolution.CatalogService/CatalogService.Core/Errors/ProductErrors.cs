namespace eCommerceCatalogService.Core.Errors;

public class CatalogProductErrors
{
    public static Error NotFound
        => Error.NotFound($"Product.{nameof(NotFound)}", "this product not found");
    public static Error Unauthorized
        => Error.Unauthorized($"Product.{nameof(Unauthorized)}", "you are not authorized to access this product");
}
