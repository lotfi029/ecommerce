using eCommerce.API.Extensions;
using eCommerce.API.Infrastracture;
using System.Reflection;

namespace eCommerce.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services)
    {
        services.AddOpenApi(op =>
            op.AddDocumentTransformer<BearerSecuritySchemeTransformer>()
        );

        services.AddEndpoints(Assembly.GetExecutingAssembly());
        services.AddExceptionHandler<GlobalExceptionHandler>();
        return services;
    }
}
