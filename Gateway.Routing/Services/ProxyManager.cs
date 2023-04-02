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

    public async Task<IReadOnlyList<RouteConfig>> GetRoutes()
    {
        return await _routingRepository.Get();
    }

    public async void AddRoute(RouteConfig route)
    {
        // TODO: Add check if route already exists
        _routingRepository.Save(route);
        
        var routes = await _routingRepository.Get();
        
        _yarpFacade.Update(routes);
    }

    public async void RemoveRoute(string route)
    {
        _routingRepository.Remove(route);
        
        var routes = await _routingRepository.Get();
        
        _yarpFacade.Update(routes);
    }
}