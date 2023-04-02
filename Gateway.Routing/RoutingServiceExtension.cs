using Asp.Versioning.Builder;
using AutoMapper;
using Gateway.Routing.Endpoints;
using Gateway.Routing.Maps;
using Gateway.Routing.Services;
using Gateway.Routing.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway.Routing;

public static class RoutingServiceExtension
{
    public static void AddRoutingService(this WebApplicationBuilder builder)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RouteConfigMaps>();
            cfg.AddProfile<YarpRouteConfigMaps>();
            cfg.AddProfile<YarpClusterConfigMaps>();
        });

        var mapper = config.CreateMapper();
        builder.Services.AddSingleton(mapper);

        builder.Services.AddSingleton<IRoutingRepository, MemoryRoutingStorage>();
        builder.Services.AddTransient<IProxyManager, ProxyManager>();
        builder.Services.AddTransient<IYarpFacade, YarpFacade>();
    }

    public static void UseRoutingService(this WebApplication app, ApiVersionSet versionSet)
    {
        app.AddRoutingEndpoints(versionSet);
    }
}