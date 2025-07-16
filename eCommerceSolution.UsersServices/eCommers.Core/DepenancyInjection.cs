using eCommers.Core.Contracts.Users;
using eCommers.Core.Profiles;
using eCommers.Core.ServiceContracts;
using eCommers.Core.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eCommers.Core;
public static class DepenancyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();

        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(typeof(MappingConfiguration).Assembly);
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>()
            .AddFluentValidationAutoValidation();

        return services;
    }
}
