using InventoryService.Core.DTOs.Inventories;
using MapsterMapper;
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

        services
            .AddValidatorsFromAssemblyContaining<InventoryRequestValidator>(includeInternalTypes: true);

        services.AddMappingAndValidation();

        return services;
    }
    private static IServiceCollection AddMappingAndValidation(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(DependancyInjection).Assembly);
        services.AddSingleton<IMapper>(new Mapper(config));


        services.AddValidatorsFromAssemblyContaining<InventoryRequestValidator>();

        return services;
    }
}
