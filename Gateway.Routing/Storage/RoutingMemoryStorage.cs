using System.Collections.Concurrent;
using Gateway.Routing.Models;

namespace Gateway.Routing.Storage;

public class MemoryRoutingStorage : IRoutingRepository
{
    private readonly ConcurrentDictionary<string, RouteConfig> _routes;
    
    public MemoryRoutingStorage()
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
        if (_routes.ContainsKey(route.Id))
        {
            _routes[route.Id] = route;
        }
        
        _routes.TryAdd(route.Id, route);
    }

    public void Remove(string key)
    {
        _routes.TryRemove(key, out _);
    }
}