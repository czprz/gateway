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

    public Task<bool> Exists(string key)
    {
        var exists = _routes.ContainsKey(key);
        return Task.FromResult(exists);
    }

    public Task<bool> Exists(RouteConfig route)
    {
        // TODO: Add more adv. check
        var exists = _routes.ContainsKey(route.Id);
        return Task.FromResult(exists);
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

    public Task<bool> Save(RouteConfig route)
    {
        if (_routes.ContainsKey(route.Id))
        {
            _routes[route.Id] = route;
        }
        
        _routes.TryAdd(route.Id, route);

        return Task.FromResult(true);
    }

    public Task<bool> Remove(string key)
    {
        if (_routes.TryRemove(key, out _))
        {
            Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}