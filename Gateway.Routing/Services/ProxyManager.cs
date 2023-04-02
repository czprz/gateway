using Gateway.Routing.Models;
using Gateway.Routing.Storage;

namespace Gateway.Routing.Services;

public class ProxyManager : IProxyManager
{
    private readonly IYarpFacade _yarpFacade;
    private readonly IRoutingRepository _routingRepository;

    public ProxyManager(IYarpFacade yarpFacade, IRoutingRepository routingRepository)
    {
        _yarpFacade = yarpFacade;
        _routingRepository = routingRepository;
    }

    public IReadOnlyList<RouteConfig> GetRoutes()
    {
        return _routingRepository.Get();
    }

    public void AddRoute(RouteConfig route)
    {
        // TODO: Add check if route already exists
        _routingRepository.Save(route);
        
        var routes = _routingRepository.Get();
        
        _yarpFacade.Update(routes);
    }

    public void RemoveRoute(string route)
    {
        _routingRepository.Remove(route);
        
        var routes = _routingRepository.Get();
        
        _yarpFacade.Update(routes);
    }
}