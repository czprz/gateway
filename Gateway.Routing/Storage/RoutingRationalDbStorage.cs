using Gateway.Routing.Storage.Rational;
using Gateway.Routing.Storage.Rational.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gateway.Routing.Storage;

public class RationalDbRoutingStorage : IRoutingRepository
{
    private readonly ILogger<RationalDbRoutingStorage> _logger;

    public RationalDbRoutingStorage(ILogger<RationalDbRoutingStorage> logger)
    {
        _logger = logger;
    }

    public async Task<bool> Exists(Guid id)
    {
        await using var db = new RouteContext();

        var anyRoute = await db.RouteConfigs.AnyAsync(x => x.Id == id);
        return anyRoute;
    }

    public async Task<bool> Exists(int hash)
    {
        await using var db = new RouteContext();

        var anyRoute = await db.RouteConfigs.AnyAsync(x => x.MatchHashCode == hash);
        return anyRoute;
    }

    public async Task<IList<RouteConfigDb>> Get()
    {
        await using var db = new RouteContext();

        var routes = await db.RouteConfigs
            .Include(x => x.Upstreams)
            .Include(x => x.Headers)
            .Include(x => x.QueryParameters)
            .Include(x => x.Hosts)
            .Include(x => x.Methods)
            .ToListAsync();
        return routes;
    }

    public async Task<RouteConfigDb?> Get(Guid id)
    {
        await using var db = new RouteContext();

        var route = await db.RouteConfigs.FirstOrDefaultAsync(x => x.Id == id);
        return route;
    }

    public async Task<RouteConfigDb?> Get(int hash)
    {
        await using var db = new RouteContext();

        var route = await db.RouteConfigs.FirstOrDefaultAsync(x => x.MatchHashCode == hash);
        return route;
    }

    public async Task<bool> Save(RouteConfigDb route)
    {
        try
        {
            await using var db = new RouteContext();
            await db.Database.EnsureCreatedAsync();

            await db.RouteConfigs.AddAsync(route);

            await db.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not save route: {Message: 0}", ex.Message);
            return false;
        }
    }

    public async Task<bool> Update(RouteConfigDb route)
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
                .Where(x => x.MatchHashCode == route.MatchHashCode)
                .FirstOrDefaultAsync();

            if (routeConfigDb == null)
            {
                return false;
            }

            routeConfigDb.UpdatedAt = DateTime.Now;

            // TODO: Missing update of non-match properties

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

    public async Task<bool> Remove(Guid id)
    {
        try
        {
            await using var db = new RouteContext();

            var item = await db.RouteConfigs.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return false;
            }

            db.RouteConfigs.Remove(item);

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