using Gateway.Routing.Models;
using Gateway.Routing.Storage.Rational;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Routing.Storage;

public class RationalDbRoutingStorage : IRoutingRepository
{
    private readonly MyDbContext _dbContext;

    public RationalDbRoutingStorage(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<RouteConfig>> Get()
    {
        var routes = await _dbContext.RouteConfigs.ToListAsync();
        return routes;
    }

    public async Task<RouteConfig?> Get(string key)
    {
        var routes = await _dbContext.RouteConfigs.FirstOrDefaultAsync(x => x.Id == key);
        return routes;
    }

    public async void Save(RouteConfig route)
    {
        await _dbContext.RouteConfigs.AddAsync(route);
    }

    public void Remove(string key)
    {
        _dbContext.RouteConfigs.Remove(new RouteConfig { Id = key });
    }
}