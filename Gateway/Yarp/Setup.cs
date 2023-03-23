using System.Collections.ObjectModel;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Yarp;

public static class Setup
{
    public static void AddReverseProxy(this WebApplicationBuilder builder)
    {
        builder.Services.AddReverseProxy()
            .LoadFromMemory(new Collection<RouteConfig>(), new Collection<ClusterConfig>());

        builder.Services.AddTransient<IYarpFacade, YarpFacade>();
    }
}