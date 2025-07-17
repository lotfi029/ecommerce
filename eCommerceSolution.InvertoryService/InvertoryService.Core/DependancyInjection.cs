using eCommerce.SharedKernal.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryService.Core;

public static class DependancyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(DependancyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );


        return services;
    }
}
