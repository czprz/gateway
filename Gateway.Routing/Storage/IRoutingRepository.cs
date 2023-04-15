using Gateway.Routing.Models;

namespace Gateway.Routing.Storage;

public interface IRoutingRepository
{
    Task<bool> Exists(string key);
    Task<bool> Exists(RouteConfig route);
    Task<IReadOnlyList<RouteConfig>> Get();
    Task<RouteConfig?> Get(string key);
    Task<RouteConfig?> Get(RouteConfig route);
    Task<bool> Save(RouteConfig route);
    Task<bool> Update(RouteConfig route);
    Task<bool> Remove(string key);
}