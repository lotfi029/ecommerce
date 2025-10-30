using CatalogService.API.Extensions;

namespace CatalogService.API;

public static class DependancyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services)
    {
        services.AddEndpoints(typeof(DependancyInjection).Assembly);
        
        return services;
    }
    
}