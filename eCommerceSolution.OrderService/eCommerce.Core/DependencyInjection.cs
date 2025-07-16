using eCommerce.Core.Contracts;
using eCommerce.Core.Profiles;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {

        services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();

        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(MappingConfiguration).Assembly);

        return services;
    }
}
