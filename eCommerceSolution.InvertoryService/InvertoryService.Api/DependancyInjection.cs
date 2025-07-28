using InventoryService.Api.Extensions;
using InventoryService.Api.Infrastracture;

namespace InventoryService.Api;

public static class DependancyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>()
        );
        services.AddEndpoints(typeof(DependancyInjection).Assembly);
        return services;
    }
}
