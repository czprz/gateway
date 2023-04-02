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
    
    public Task<IReadOnlyList<RouteConfig>> Get()
    {
        var routes = (IReadOnlyList<RouteConfig>) _routes.Values.ToList().AsReadOnly();
        return Task.FromResult(routes);
    }

    public Task<RouteConfig?> Get(string key)
    {
        var routeValue =_routes.TryGetValue(key, out var route) ? route : null;
        return Task.FromResult(routeValue);
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