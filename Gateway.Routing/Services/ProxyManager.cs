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

    public async Task<IReadOnlyList<RouteConfig>> Get()
    {
        return await _routingRepository.Get();
    }

    public async void Start()
    {
        var routes = await _routingRepository.Get();
        _yarpFacade.Update(routes);
    }

    public async Task<ProxyManagerResult> Add(RouteConfig route)
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

    public async Task<ProxyManagerResult> Update(RouteConfig route)
    {
        if (!await _routingRepository.Exists(route))
        {
            return ProxyManagerResult.NotFound;
        }

        if (!await _routingRepository.Update(route))
        {
            return ProxyManagerResult.Error;
        }

        return ProxyManagerResult.Ok;
    }

    public async Task<ProxyManagerResult> Remove(string key)
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