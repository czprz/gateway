using Gateway.Routing.Storage.Rational.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Routing.Storage.Rational;

public class RouteContext : DbContext
{
    public DbSet<RouteConfigDb> RouteConfigs { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=localhost;Database=Gateway;User Id=sa;Password=123456789Qwerty;TrustServerCertificate=True");
}