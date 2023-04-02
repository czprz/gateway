using Gateway.Routing.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Routing.Storage.Rational;

public class MyDbContext : DbContext
{
    public DbSet<RouteConfig> RouteConfigs { get; set; }
}