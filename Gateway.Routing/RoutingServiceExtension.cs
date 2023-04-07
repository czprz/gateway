using Asp.Versioning.Builder;
using AutoMapper;
using Gateway.Common.Config;
using Gateway.Routing.Endpoints;
using Gateway.Routing.Maps;
using Gateway.Routing.Services;
using Gateway.Routing.Storage;
using Gateway.Routing.Storage.Rational;
using Gateway.Routing.Storage.Rational.Maps;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway.Routing;

public static class RoutingServiceExtension
{
    public static void AddRoutingService(this WebApplicationBuilder builder)
    {
        var config = builder.Services.BuildServiceProvider().GetService<IConfig>();

        var mapConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RouteConfigDbMaps>();
            cfg.AddProfile<RouteConfigMaps>();
            cfg.AddProfile<YarpRouteConfigMaps>();
            cfg.AddProfile<YarpClusterConfigMaps>();
        });

        var mapper = mapConfig.CreateMapper();
        builder.Services.AddSingleton(mapper);

        ChooseRoutingStorage(builder, config!);

        builder.Services.AddTransient<IProxyManager, ProxyManager>();
        builder.Services.AddTransient<IYarpFacade, YarpFacade>();
    }

    public static void UseRoutingService(this WebApplication app, ApiVersionSet versionSet)
    {
        app.AddRoutingEndpoints(versionSet);
    }

    private static void ChooseRoutingStorage(WebApplicationBuilder builder, IConfig config)
    {
        switch (config.StorageType)
        {
            case StorageType.SqlServer:
            {
                builder.Services.AddTransient<IRoutingRepository, RationalDbRoutingStorage>();
                
                using var dbContext = new RouteContext();
                dbContext.Database.Migrate();

                break;
            }
            default:
                builder.Services.AddSingleton<IRoutingRepository, MemoryRoutingStorage>();
                break;
        }
    }
}