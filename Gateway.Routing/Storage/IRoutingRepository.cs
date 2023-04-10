using Gateway.Routing.Models;

namespace Gateway.Routing.Storage;

public interface IRoutingRepository
{
    Task<bool> Exists(string key);
    Task<bool> Exists(RouteConfig route);
    Task<bool> Exists(IEnumerable<RouteConfig> routes);
    Task<IReadOnlyList<RouteConfig>> Get();
    Task<RouteConfig?> Get(string key);
    Task<bool> Save(RouteConfig route);
    Task<bool> Save(IEnumerable<RouteConfig> routes);
    Task<bool> Update(RouteConfig route);
    Task<bool> Update(IEnumerable<RouteConfig> routes);
    Task<bool> Remove(string key);
}