using eCommerce.API.Middlewares;
using eCommerce.Infrastructure;
using eCommers.Core;

namespace eCommerce.API;
public static class DepenancyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAnotherLayers(configuration);

        services.AddExceptionHandler<ExceptionHanldingMiddleware>();
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        
        services.AddControllers();
        return services;
    }

    private static IServiceCollection AddAnotherLayers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddCore(configuration);
        return services;
    }
}
