using Gateway.Routing.Models;

namespace Gateway.Routing.Services;

public interface IProxyManager
{
    Task<IReadOnlyList<RouteConfig>> GetRoutes();
    void AddRoute(RouteConfig route);
    void RemoveRoute(string route);
}