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
        return Task.FromResult(_routes.ContainsKey(route.Id) ||
                               _routes.Any(x => x.Value.GetMatchHash() == route.GetMatchHash()));
    }

    public Task<bool> Exists(IEnumerable<RouteConfig> routes)
    {
        var keys = routes.Select(x => x.Id);
        return Task.FromResult(_routes.Keys.Any(key => keys.Contains(key)) ||
                               _routes.Values.Any(rc2 => routes.Any(rc => rc.GetMatchHash() == rc2.GetMatchHash())));
    }

    public Task<IReadOnlyList<RouteConfig>> Get()
    {
        var routes = (IReadOnlyList<RouteConfig>)_routes.Values.ToList().AsReadOnly();
        return Task.FromResult(routes);
    }

    public Task<RouteConfig?> Get(string key)
    {
        var routeValue = _routes.TryGetValue(key, out var route) ? route : null;
        return Task.FromResult(routeValue);
    }

    public Task<RouteConfig?> Get(RouteConfig route)
    {
        var routeValue = _routes.TryGetValue(route.Id, out var route2) ? route2 : null;
        return Task.FromResult(routeValue);
    }

    public Task<IReadOnlyList<RouteConfig>> Get(IEnumerable<RouteConfig> routes)
    {
        // TODO: Check hashcode
        var routeConfigs = routes.Select(x => _routes.TryGetValue(x.Id, out var route) ? route : null).ToList();
        return Task.FromResult((IReadOnlyList<RouteConfig>)routeConfigs.AsReadOnly());
    }

    public Task<bool> Save(RouteConfig route)
    {
        _routes.TryAdd(route.Id, route);

        return Task.FromResult(true);
    }

    public Task<bool> Save(IEnumerable<RouteConfig> routes)
    {
        foreach (var route in routes)
        {
            _routes.TryAdd(route.Id, route);
        }

        return Task.FromResult(true);
    }

    public Task<bool> Update(RouteConfig route)
    {
        _routes.TryUpdate(route.Id, route, route);

        return Task.FromResult(true);
    }

    public Task<bool> Update(IEnumerable<RouteConfig> routes)
    {
        foreach (var route in routes)
        {
            _routes.TryUpdate(route.Id, route, route);
        }

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