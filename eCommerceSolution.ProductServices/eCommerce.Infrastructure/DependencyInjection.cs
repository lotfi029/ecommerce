using eCommerce.Core.IRabbitMQ;
using eCommerce.Core.IRepositoryContracts;
using eCommerce.Infrastructure.Authentication;
using eCommerce.Infrastructure.Extensions;
using eCommerce.Infrastructure.Presistense;
using eCommerce.Infrastructure.RabbitMQ;
using eCommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace eCommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        
        var connectiontemp = configuration.GetConnectionStringOrThrow("NpgsqlConnection");

        string connectionString = connectiontemp
            .Replace("$POSTGRES_HOST", Environment.GetEnvironmentVariable("POSTGRES_HOST"))
            .Replace("$POSTGRES_PORT", Environment.GetEnvironmentVariable("POSTGRES_PORT"))
            .Replace("$POSTGRES_DATABASE", Environment.GetEnvironmentVariable("POSTGRES_DATABASE"))
            .Replace("$POSTGRES_USER", Environment.GetEnvironmentVariable("POSTGRES_USER"))
            .Replace("$POSTGRES_PASSWORD", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"));
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<DapperDbContext>();
        services.AddAuth(configuration);
        services.AddRabbitMQ();
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
    private static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        services.AddOptions<RabbitMQOptions>()
            .BindConfiguration(RabbitMQOptions.SectionsName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();

        //services.AddMassTransit(busConfigure =>
        //{
        //    busConfigure.SetKebabCaseEndpointNameFormatter();

        //    busConfigure.UsingRabbitMq((context, config) =>
        //    {
        //        var settings = context.GetRequiredService<RabbitMQOptions>();

        //        config.Host(new Uri($"emqp://{settings.HostName}:{settings.Port}"), h =>
        //        {
        //            h.Username(settings.UserName);
        //            h.Password(settings.Password);
        //        });
        //    });
        //});


        return services;
    }

}
