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

    public async void AddRoutes()
    {
        var routes = await _routingRepository.Get();
        _yarpFacade.Update(routes);
    }

    public async Task<ProxyManagerResult> AddRoute(RouteConfig route)
    {
        if (await _routingRepository.Exists(route))
        {
            return ProxyManagerResult.AlreadyExists;
        }
        
        if (!await _routingRepository.Save(route))
        {
            return ProxyManagerResult.Error;
        }

        var routes = await _routingRepository.Get();

        return _yarpFacade.Update(routes)
            ? ProxyManagerResult.Ok
            : ProxyManagerResult.Error;
    }

    public async Task<ProxyManagerResult> RemoveRoute(string key)
    {
        if (!await _routingRepository.Exists(key))
        {
            return ProxyManagerResult.NotFound;
        }

        if (await _routingRepository.Remove(key))
        {
            return ProxyManagerResult.Error;
        }

        var routes = await _routingRepository.Get();

        return _yarpFacade.Update(routes)
            ? ProxyManagerResult.Ok
            : ProxyManagerResult.Error;
    }
}