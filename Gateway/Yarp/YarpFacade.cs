using AutoMapper;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Yarp;

public class YarpFacade : IYarpFacade
{
    private readonly InMemoryConfigProvider _configProvider;
    private readonly IMapper _mapper;

    public YarpFacade(InMemoryConfigProvider configProvider, IMapper mapper)
    {
        _configProvider = configProvider;
        _mapper = mapper;
    }

    public void Update(Config.RouteConfig route)
    {
        var clusterConfig = _mapper.Map<Config.RouteConfig, ClusterConfig>(route);
        var routeConfig =
            _mapper.Map<Config.RouteConfig, RouteConfig>(route,
                opt => opt.Items["ClusterId"] = clusterConfig.ClusterId);

        _configProvider.Update(new[] { routeConfig }, new[] { clusterConfig });
    }

    public IReadOnlyList<Config.RouteConfig> Read()
    {
        var routes = _configProvider.GetConfig().Routes;

        return routes.Select(_mapper.Map<RouteConfig, Config.RouteConfig>).ToList().AsReadOnly();
    }
}