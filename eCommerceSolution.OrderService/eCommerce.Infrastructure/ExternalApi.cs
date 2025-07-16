using System.ComponentModel.DataAnnotations;

namespace eCommerce.Infrastructure;
public class ExternalApi
{
    [Required]
    public static string UsersApi 
    {
        get
        {
            return Environment.GetEnvironmentVariable("USER_SERVICE_URL")!;
        }

    }
    [Required]
    public static string ProductsApi 
    {
        get 
        {
            return Environment.GetEnvironmentVariable("PRODUCT_SERVICE_URL")!;
        } 
    }

}