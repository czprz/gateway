using Gateway.Routing.Models;

namespace Gateway.Routing.Storage;

public interface IRoutingRepository
{
    Task<IReadOnlyList<RouteConfig>> Get();
    Task<RouteConfig?> Get(string key);
    void Save(RouteConfig route);
    void Remove(string key);
}