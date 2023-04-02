using Gateway.Routing.Models;

namespace Gateway.Routing.Services;

public interface IYarpFacade
{
    void Update(IEnumerable<RouteConfig> routes);
    IReadOnlyList<RouteConfig> Read();
}