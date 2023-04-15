using AutoMapper;
using Gateway.Routing.Models;
using Gateway.Routing.Storage.Rational;
using Gateway.Routing.Storage.Rational.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gateway.Routing.Storage;

public class RationalDbRoutingStorage : IRoutingRepository
{
    private readonly IMapper _mapper;
    private readonly ILogger<RationalDbRoutingStorage> _logger;

    public RationalDbRoutingStorage(IMapper mapper, ILogger<RationalDbRoutingStorage> logger)
    {
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<bool> Exists(string key)
    {
        await using var db = new RouteContext();

        var anyRoute = await db.RouteConfigs.AnyAsync(x => x.Id == Guid.Parse(key));
        return anyRoute;
    }

    public async Task<bool> Exists(RouteConfig route)
    {
        await using var db = new RouteContext();

        var anyRoute = await db.RouteConfigs.AnyAsync(x => x.MatchHashCode == route.GetMatchHash());
        return anyRoute;
    }

    public async Task<bool> Exists(IEnumerable<RouteConfig> routes)
    {
        await using var db = new RouteContext();

        var hashes = await db.RouteConfigs.Select(x => x.MatchHashCode).ToListAsync();
        var anyRoutes = routes.All(x => hashes.Contains(x.GetMatchHash()));
        return anyRoutes;
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

        var routeConfigDb = await db.RouteConfigs.Include(x => x.Headers)
            .FirstOrDefaultAsync(x => x.Id.ToString() == key);
        if (routeConfigDb == null)
        {
            return null;
        }

        var route = _mapper.Map<RouteConfig>(routeConfigDb);

        return route;
    }

    public async Task<RouteConfig?> Get(RouteConfig route)
    {
        try
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
                .Where(x => x.MatchHashCode == route.GetMatchHash())
                .FirstOrDefaultAsync();

            var routes = _mapper.Map<RouteConfig>(routeConfigDbs);
            return routes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not get route: {Message: 0}", ex.Message);
            return await Task.FromResult<RouteConfig?>(null);
        }
    }

    public async Task<bool> Save(RouteConfig route)
    {
        try
        {
            await using var db = new RouteContext();
            await db.Database.EnsureCreatedAsync();

            var routeConfig = _mapper.Map<RouteConfigDb>(route);
            await db.RouteConfigs.AddAsync(routeConfig);

            await db.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not save route: {Message: 0}", ex.Message);
            return false;
        }
    }

    public async Task<bool> Update(RouteConfig route)
    {
        try
        {
            await using var db = new RouteContext();
            await db.Database.EnsureCreatedAsync();

            var routeConfigDb = await db.RouteConfigs
                .Include(x => x.Hosts)
                .Include(x => x.Methods)
                .Include(x => x.Headers)
                .ThenInclude(o => o.Values)
                .Include(x => x.QueryParameters)
                .ThenInclude(o => o.Values)
                .Include(x => x.Upstreams)
                .Where(x => x.MatchHashCode == route.GetMatchHash())
                .FirstOrDefaultAsync();

            if (routeConfigDb == null)
            {
                return false;
            }
            
            routeConfigDb.UpdatedAt = DateTime.Now;

            db.RouteConfigs.Update(routeConfigDb);

            await db.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not update route: {Message: 0}", ex.Message);
            return false;
        }
    }

    public async Task<bool> Remove(string key)
    {
        try
        {
            await using var db = new RouteContext();
            
            var id = Guid.Parse(key);
            var itemToRemove = await db.RouteConfigs.FindAsync(id);
            if (itemToRemove == null)
            {
                return false;
            }
            
            db.RouteConfigs.Remove(itemToRemove);

            await db.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not remove route: {Message: 0}", ex.Message);
            return false;
        }
    }
}