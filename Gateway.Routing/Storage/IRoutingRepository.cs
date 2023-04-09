using Gateway.Routing.Models;

namespace Gateway.Routing.Storage;

public interface IRoutingRepository
{
    Task<bool> Exists(string key);
    Task<bool> Exists(RouteConfig route);
    Task<IReadOnlyList<RouteConfig>> Get();
    Task<RouteConfig?> Get(string key);
    Task<bool> Save(RouteConfig route);
    Task<bool> Remove(string key);
}