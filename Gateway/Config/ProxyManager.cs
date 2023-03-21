using AutoMapper;
using Yarp.ReverseProxy.Configuration;

namespace Yarp.Config;

public class ProxyManager : IProxyManager
{
    private readonly InMemoryConfigProvider _configProvider;
    private readonly IMapper _mapper;

    public ProxyManager(InMemoryConfigProvider configProvider, IMapper mapper)
    {
        _configProvider = configProvider;
        _mapper = mapper;
    }

    public IReadOnlyList<Mapping.RouteConfig> GetRoutes()
    {
        return _configProvider.GetConfig().Routes.Select(_mapper.Map<RouteConfig, Mapping.RouteConfig>).ToList().AsReadOnly();
    }

    public void AddRoute(Mapping.RouteConfig route)
    {
        var routeConfig = _mapper.Map<Mapping.RouteConfig, RouteConfig>(route);
        var cluster = new ClusterConfig();
        
        _configProvider.Update(new[] { routeConfig }, new[] { cluster });
    }

    public void RemoveRoute(string route)
    {
        throw new NotImplementedException();
    }
}