using Gateway.Routing.Models;

namespace Gateway.Routing.Services;

public interface IProxyManager
{
    void Start();
    Task<IReadOnlyList<RouteConfig>> Get();
    Task<ProxyManagerResult> Add(IList<RouteConfig> routes);
    Task<ProxyManagerResult> Add(RouteConfig route);
    Task<ProxyManagerResult> Update(IList<RouteConfig> routes);
    Task<ProxyManagerResult> Remove(string key);
}