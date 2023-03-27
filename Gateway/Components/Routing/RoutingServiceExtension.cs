using AutoMapper;
using Gateway.Components.Routing.Endpoints;
using Gateway.Components.Routing.Maps;
using Gateway.Components.Routing.Services;

namespace Gateway.Components.Routing;

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

        builder.Services.AddSingleton<IRoutingRepository, RoutingRepository>();
        builder.Services.AddTransient<IProxyManager, ProxyManager>();
        builder.Services.AddTransient<IYarpFacade, YarpFacade>();
    }

    public static void UseRoutingService(this WebApplication app)
    {
        app.AddRoutingEndpoints();
    }
}