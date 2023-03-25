using System.Collections.Concurrent;

namespace Gateway.Components.Routing.Services;

public class RoutingRepository : IRoutingRepository
{
    private readonly ConcurrentDictionary<string, RouteConfig> _routes;
    
    public RoutingRepository()
    {
        _routes = new ConcurrentDictionary<string, RouteConfig>();
    }
    
    public IReadOnlyList<RouteConfig> Get()
    {
        return _routes.Values.ToList();
    }

    public RouteConfig? Get(string key)
    {
        return _routes.TryGetValue(key, out var route) ? route : null;
    }

    public void Save(RouteConfig route)
    {
        if (_routes.ContainsKey(route.Path!))
        {
            _routes[route.Path!] = route;
        }
        
        _routes.TryAdd(route.Path!, route);
    }

    public void Remove(string key)
    {
        _routes.TryRemove(key, out _);
    }
}