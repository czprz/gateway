using Gateway.Routing.Models;

namespace Gateway.Routing.Services;

public interface IProxyManager
{
    Task<IReadOnlyList<RouteConfig>> GetRoutes();
    void AddRoutes();
    Task<ProxyManagerResult> AddRoute(RouteConfig route);
    Task<ProxyManagerResult> RemoveRoute(string key);
}