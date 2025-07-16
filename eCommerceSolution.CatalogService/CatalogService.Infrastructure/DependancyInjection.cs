using eCommerceCatalogService.Infrastructure.Authentication;
using eCommerceCatalogService.Infrastructure.Clients;
using eCommerceCatalogService.Infrastructure.RabbitMQ;
using eCommerceCatalogService.Infrastructure.RabbitMQ.Consumers;
using eCommerceCatalogService.Infrastructure.Repositories;
using eCommerceCatalogService.Infrastructure.Settings;
using eCommerceCatalogService.Core.IClients;
using eCommerceCatalogService.Core.IRepositories;
using eCommerceCatalogService.Infrastructure.Extensions;
using eCommerceCatalogService.Infrastructure.RabbitMQ.HostedServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using eCommerceCatalogService.Core.IConsumers;
using CatalogService.Infrastructure.Persistense;

namespace eCommerceCatalogService.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddInfractructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRabbitMQ();
        services.AddService();

        var connectionString = configuration.GetConnectionStringOrThrow("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddClients();
        services.AddAuth(configuration);
        return services;
    }
    private static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        services.AddOptions<RabbitMQSettings>()
            .BindConfiguration(RabbitMQSettings.SectionsName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddTransient<IRabbitMQProductAddedConsumer, RabbitMQProductAddedConsumer>();

        services.AddHostedService<RabbitMQProductHostedService>();
        return services;
    }

    private static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddScoped<ICatalogRepository, CatalogRepository>();

        return services;
    }
    private static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddHttpClient<IProductClient,ProductClient> (client =>
        {
            client.BaseAddress = new Uri(ExternalApiSettings.ProductAPI);
        });
        

        return services;
    }
    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
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

        services.AddAuthorization();
        return services;
    }
}