using System.Collections.Concurrent;
using Gateway.Routing.Storage.Rational.Models;

namespace Gateway.Routing.Storage;

public class MemoryRoutingStorage : IRoutingRepository
{
    private readonly ConcurrentDictionary<string, RouteConfigDb> _routes;

    public MemoryRoutingStorage()
    {
        _routes = new ConcurrentDictionary<string, RouteConfigDb>();
    }


    public Task<bool> Exists(Guid id)
    {
        var exists = _routes.ContainsKey(id.ToString());
        return Task.FromResult(exists);
    }

    public Task<bool> Exists(int hash)
    {
        var exists = _routes.Values.Any(x => x.MatchHashCode == hash);
        return Task.FromResult(exists);
    }

    public Task<IList<RouteConfigDb>> Get()
    {
        var routes = _routes.Values.ToList();
        return Task.FromResult((IList<RouteConfigDb>)routes);
    }

    public Task<RouteConfigDb?> Get(Guid id)
    {
        var route = _routes.TryGetValue(id.ToString(), out var routeConfigDb) ? routeConfigDb : null;
        return Task.FromResult(route);
    }

    public Task<RouteConfigDb?> Get(int hash)
    {
        var route = _routes.Values.FirstOrDefault(x => x.MatchHashCode == hash);
        return Task.FromResult(route);
    }

    public Task<bool> Save(RouteConfigDb route)
    {
        var added = _routes.TryAdd(route.Id.ToString(), route);
        return Task.FromResult(added);
    }

    public Task<bool> Update(RouteConfigDb route)
    {
        var updated = _routes.TryUpdate(route.Id.ToString(), route, route);
        return Task.FromResult(updated);
    }

    public Task<bool> Remove(Guid id)
    {
        var removed = _routes.TryRemove(id.ToString(), out _);
        return Task.FromResult(removed);
    }
}