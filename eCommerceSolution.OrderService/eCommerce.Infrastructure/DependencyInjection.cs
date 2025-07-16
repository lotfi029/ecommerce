using eCommerce.Core.IRepositoryContracts;
using eCommerce.Core.ServiceContracts;
using eCommerce.Infrastructure.Extensions;
using eCommerce.Infrastructure.HttpClients;
using eCommerce.Infrastructure.PollyPolicies;
using eCommerce.Infrastructure.Repositories;
using eCommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

namespace eCommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionTemplate = configuration.GetConnectionStringOrThrow("MongoDbConnection");

        var connection = connectionTemplate
            .Replace("$MONGO_HOST", Environment.GetEnvironmentVariable("MONGO_HOST"))
            .Replace("$MONGO_PORT", Environment.GetEnvironmentVariable("MONGO_PORT"));

        services.AddSingleton<IMongoClient>(new MongoClient(connection));
        Console.WriteLine(connection);
        services.AddScoped(provider =>
        {
            var client = provider.GetRequiredService<IMongoClient>();

            return client.GetDatabase("eCommerceOrder");
        });

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrdersService, OrdersService>();
        services.AddAuth(configuration);
        services.AddSyncCommunicationService();
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

        return services;
    }
    private static IServiceCollection AddSyncCommunicationService(this IServiceCollection services)
    {

        services.AddTransient<IUserClientPollyPolicies, UserClientPollyPolicies>();
        services.AddTransient<IProductClientPollyPolicies, ProductClientPollyPolicies>();
        services.AddTransient<IReusablePollyPolicies, ReusablePollyPolicies>();


        services.AddHttpClient<UsersHttpClient>(c =>
        {
            c.BaseAddress = new Uri(ExternalApi.UsersApi);
        }).AddPolicyHandler(
            services.BuildServiceProvider().GetRequiredService<IUserClientPollyPolicies>().GetCombinedPolicy()
            );

        services.AddHttpClient<ProductHttpClient>(c =>
        {
            c.BaseAddress = new Uri(ExternalApi.ProductsApi);
        }).AddPolicyHandler(
            services.BuildServiceProvider().GetRequiredService<IUserClientPollyPolicies>().GetCombinedPolicy()
            ).AddPolicyHandler(
            services.BuildServiceProvider().GetRequiredService<IProductClientPollyPolicies>().GetCombinedPolicy
            );

        return services;
    }
}