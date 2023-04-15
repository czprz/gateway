using Gateway.Routing.Models;

namespace Gateway.Routing.Services;

public interface IProxyManager
{
    void Start();
    Task<IReadOnlyList<RouteConfig>> Get();
    Task<ProxyManagerResult> Add(RouteConfig route);
    Task<ProxyManagerResult> Update(RouteConfig route);
    Task<ProxyManagerResult> Remove(string key);
}