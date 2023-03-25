namespace Gateway.Components.Routing.Services;

public interface IRoutingRepository
{
    IReadOnlyList<RouteConfig> Get();
    RouteConfig? Get(string key);
    void Save(RouteConfig route);
    void Remove(string key);
}