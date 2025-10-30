using Microsoft.Extensions.Configuration;

namespace CatalogService.Infrastructure.Extensions;
public static class ConfigurationExtensions
{
    public static string GetConnectionStringOrThrow(this IConfiguration configuration, string name = "DefaultConnection")
        => configuration.GetConnectionString(name) 
        ?? throw new InvalidOperationException($"Connection string '{name}' not found in configuration.");
}
