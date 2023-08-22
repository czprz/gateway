using Gateway.Routing.Models;

namespace Gateway.Routing.Services;

public interface IProxyFacade
{
    Task<IReadOnlyList<RouteConfig>> Get();
    Task<RouteConfig?> Get(string key);
    Task<ProxyManagerResult> Add(RouteConfig route);
    Task<ProxyManagerResult> Update(Guid routeId, RouteConfig route);
    Task<ProxyManagerResult> Remove(string key);
}