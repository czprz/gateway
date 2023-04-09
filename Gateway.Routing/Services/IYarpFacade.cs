using Gateway.Routing.Models;

namespace Gateway.Routing.Services;

public interface IYarpFacade
{
    bool Update(IEnumerable<RouteConfig> routes);
    IReadOnlyList<RouteConfig> Read();
}