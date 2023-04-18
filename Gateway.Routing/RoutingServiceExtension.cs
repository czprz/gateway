using Asp.Versioning.Builder;
using Gateway.Common.Config;
using Gateway.Routing.Endpoints;
using Gateway.Routing.Hosted;
using Gateway.Routing.Services;
using Gateway.Routing.Storage;
using Gateway.Routing.Storage.Rational;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway.Routing;

public static class RoutingServiceExtension
{
    public static void AddRoutingService(this WebApplicationBuilder builder)
    {
        var config = builder.Services.BuildServiceProvider().GetService<IConfig>();

        ChooseRoutingStorage(builder, config!);

        builder.Services.AddTransient<IProxyFacade, ProxyFacade>();
        builder.Services.AddTransient<IYarpFacade, YarpFacade>();

        builder.Services.AddHostedService<ProxyManagerService>();
        builder.Services.AddHostedService<RoutingMaintainerService>();
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
                
                StorageConnectionString.Set(config.StorageConnectionString!);
                
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