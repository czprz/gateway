using Gateway.Components.Routing.Models;

namespace Gateway.Components.Routing.Services;

public interface IProxyManager
{
    IReadOnlyList<RouteConfig> GetRoutes();
    void AddRoute(RouteConfig route);
    void RemoveRoute(string route);
}