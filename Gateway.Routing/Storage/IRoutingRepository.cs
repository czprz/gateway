using Gateway.Routing.Storage.Rational.Models;

namespace Gateway.Routing.Storage;

public interface IRoutingRepository
{
    Task<bool> Exists(Guid id);
    Task<bool> Exists(int hash);
    Task<IList<RouteConfigDb>> Get();
    Task<RouteConfigDb?> Get(Guid id);
    Task<RouteConfigDb?> Get(int hash);
    Task<bool> Save(RouteConfigDb route);
    Task<bool> Update(RouteConfigDb route);
    Task<bool> Remove(Guid id);
}