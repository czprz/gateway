using Gateway.Components.Routing.Models;

namespace Gateway.Components.Routing.Services;

public interface IYarpFacade
{
    void Update(IEnumerable<RouteConfig> routes);
    IReadOnlyList<RouteConfig> Read();
}