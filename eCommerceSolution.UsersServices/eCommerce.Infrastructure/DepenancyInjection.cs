using eCommerce.Infrastructure.Authentication;
using eCommerce.Infrastructure.Presistense;
using eCommerce.Infrastructure.Presistense.Migrations;
using eCommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace eCommerce.Infrastructure;
public static class DepenancyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<DapperDbContext>();
        services.AddAuth(configuration);
        return services;
    }
    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddOptions<JwtOptions>()
            .BindConfiguration(nameof(JwtOptions))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var settings = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings!.Key)),
                ValidIssuer = settings.Issuer,
                ValidAudience = settings.Audience,
            };
        });

        return services;
    }

    private static async Task<IServiceCollection> EnsurePostgreSqlDatabaseAsync(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddScoped<DataBaseMigrator>();
        var dbContext = new DapperDbContext(configuration);
        var migrator = new DataBaseMigrator(dbContext);
        await migrator.CreateTablesAsync();
        await migrator.SeedDefaultDataAsync();
        return service;
    }
}
