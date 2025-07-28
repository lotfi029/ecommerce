﻿using InventoryService.Api.Endpoints;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace InventoryService.Api.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] endpoints = [.. assembly
            .DefinedTypes
            .Where(type => type.IsClass && !type.IsInterface && !type.IsAbstract && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))];


        services.TryAddEnumerable(endpoints);

        return services;
    }


    public static IApplicationBuilder MapEndpoints (
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null
        )
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
}
