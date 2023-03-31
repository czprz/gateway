using AutoMapper;
using Gateway.Config;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Components.Routing.Services;

public class YarpFacade : IYarpFacade
{
    private readonly InMemoryConfigProvider _configProvider;
    private readonly IMapper _mapper;
    private readonly IConfig _config;

    public YarpFacade(InMemoryConfigProvider configProvider, IMapper mapper, IConfig config)
    {
        _configProvider = configProvider;
        _mapper = mapper;
        _config = config;
    }

    public void Update(IEnumerable<RouteConfig> routes)
    {
        var routeConfigs = new List<Yarp.ReverseProxy.Configuration.RouteConfig>();
        var clusterConfigs = new List<ClusterConfig>();
        
        foreach (var route in routes)
        {
            var clusterConfig = _mapper.Map<RouteConfig, ClusterConfig>(route);
            var routeConfig = _mapper.Map<RouteConfig, Yarp.ReverseProxy.Configuration.RouteConfig>(route,
                opt => ConfigureValuesForMapping(opt, clusterConfig, route));
            
            routeConfigs.Add(routeConfig);
            clusterConfigs.Add(clusterConfig);
        }

        _configProvider.Update(routeConfigs, clusterConfigs);
    }

    public IReadOnlyList<RouteConfig> Read()
    {
        var routes = _configProvider.GetConfig().Routes;

        return routes.Select(_mapper.Map<Yarp.ReverseProxy.Configuration.RouteConfig, RouteConfig>).ToList().AsReadOnly();
    }

    private void ConfigureValuesForMapping(IMappingOperationOptions opt, ClusterConfig clusterConfig, RouteConfig routeConfig)
    {
        var authReq = routeConfig.UseAuthentication ?? false;
        
        opt.Items["ClusterId"] = clusterConfig.ClusterId;
        opt.Items["AuthorizationPolicy"] = _config.AuthFlowEnabled && authReq ? _config.AuthFlowKey : null;
    }
}