using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerceCatalogService.Core;

public static class DependancyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(DependancyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(DependancyInjection).Assembly);


        //services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingDecorators.QueryHandler<,>));
        //services.Decorate(typeof(ICommandHandler<>), typeof(LoggingDecorators.CommandHandler<>));

        return services;
    }
}