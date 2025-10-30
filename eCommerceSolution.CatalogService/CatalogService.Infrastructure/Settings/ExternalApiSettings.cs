namespace CatalogService.Infrastructure.Settings;

public class ExternalApiSettings
{
    public static string ProductAPI
        => Environment.GetEnvironmentVariable("Product_Service_URI")!;
}
