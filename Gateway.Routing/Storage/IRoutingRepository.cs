using Gateway.Routing.Models;

namespace Gateway.Routing.Storage;

public interface IRoutingRepository
{
    IReadOnlyList<RouteConfig> Get();
    RouteConfig? Get(string key);
    void Save(RouteConfig route);
    void Remove(string key);
}