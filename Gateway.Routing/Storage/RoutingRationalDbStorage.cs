using AutoMapper;
using Gateway.Routing.Models;
using Gateway.Routing.Storage.Rational;
using Gateway.Routing.Storage.Rational.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Routing.Storage;

public class RationalDbRoutingStorage : IRoutingRepository
{
    private readonly IMapper _mapper;

    public RationalDbRoutingStorage(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<RouteConfig>> Get()
    {
        await using var db = new RouteContext();

        var routeConfigDbs = await db.RouteConfigs
            .Include(x => x.Hosts)
            .Include(x => x.Methods)
            .Include(x => x.Headers)
                .ThenInclude(o => o.Values)
            .Include(x => x.QueryParameters)
                .ThenInclude(o => o.Values)
            .Include(x => x.Upstreams)
            .ToListAsync();
        
        var routes = _mapper.Map<IReadOnlyList<RouteConfig>>(routeConfigDbs);

        return routes;
    }

    public async Task<RouteConfig?> Get(string key)
    {
        await using var db = new RouteContext();

        var routeConfigDb = await db.RouteConfigs.
            Include(x => x.Headers)
            .FirstOrDefaultAsync(x => x.Id.ToString() == key);
        if (routeConfigDb == null)
        {
            return null;
        }

        var route = _mapper.Map<RouteConfig>(routeConfigDb);

        return route;
    }

    public async void Save(RouteConfig route)
    {
        await using var db = new RouteContext();
        await db.Database.EnsureCreatedAsync();

        var routeConfig = _mapper.Map<RouteConfigDb>(route);
        await db.RouteConfigs.AddAsync(routeConfig);

        await db.SaveChangesAsync();
    }

    public async void Remove(string key)
    {
        await using var db = new RouteContext();
        await db.Database.EnsureDeletedAsync();

        db.RouteConfigs.Remove(new RouteConfigDb { Id = Guid.Parse(key) });
    }
}