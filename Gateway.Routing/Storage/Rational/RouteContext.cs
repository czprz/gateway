using Gateway.Routing.Storage.Rational.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Routing.Storage.Rational;

public class RouteContext : DbContext
{
    public DbSet<RouteConfigDb> RouteConfigs { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(StorageConnectionString.Get());
}