using Gateway.Routing.Models;

namespace Gateway.Routing.Services;

public interface IProxyManager
{
    void Refresh();
    Task<IReadOnlyList<RouteConfig>> GetRoutes();
    void AddRoute(RouteConfig route);
    void RemoveRoute(string route);
}