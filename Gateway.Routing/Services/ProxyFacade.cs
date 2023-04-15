using AutoMapper;
using Gateway.Common.Extensions;
using Gateway.Routing.Models;
using Gateway.Routing.Storage;
using Gateway.Routing.Storage.Rational.Models;

namespace Gateway.Routing.Services;

public class ProxyFacade : IProxyFacade
{
    private readonly IYarpFacade _yarpFacade;
    private readonly IRoutingRepository _routingRepository;
    private readonly IMapper _mapper;

    public ProxyFacade(IYarpFacade yarpFacade, IRoutingRepository routingRepository, IMapper mapper)
    {
        _yarpFacade = yarpFacade;
        _routingRepository = routingRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<RouteConfig>> Get()
    {
        var routes = await _routingRepository.Get();
        return _mapper.Map<IReadOnlyList<RouteConfig>>(routes);
    }

    public async Task<RouteConfig?> Get(string key)
    {
        var id = key.ToGuid();
        
        var route = await _routingRepository.Get(id);
        var routeConfig = _mapper.Map<RouteConfig>(route);

        return routeConfig;
    }

    public async Task<ProxyManagerResult> Add(RouteConfig route)
    {
        var hash = route.GetHashCode();
        if (await _routingRepository.Exists(hash))
        {
            return ProxyManagerResult.AlreadyExists;
        }

        var routeDb = _mapper.Map<RouteConfigDb>(route);
        if (!await _routingRepository.Save(routeDb))
        {
            return ProxyManagerResult.Error;
        }

        var routes = await _routingRepository.Get();
        var routeConfigs = _mapper.Map<IReadOnlyList<RouteConfig>>(routes);

        return _yarpFacade.Update(routeConfigs)
            ? ProxyManagerResult.Ok
            : ProxyManagerResult.Error;
    }

    public async Task<ProxyManagerResult> Update(RouteConfig route)
    {
        var hash = route.GetHashCode();
        if (!await _routingRepository.Exists(hash))
        {
            return ProxyManagerResult.NotFound;
        }

        var routeDb = _mapper.Map<RouteConfigDb>(route);
        if (!await _routingRepository.Update(routeDb))
        {
            return ProxyManagerResult.Error;
        }

        return ProxyManagerResult.Ok;
    }

    public async Task<ProxyManagerResult> Remove(string key)
    {
        var id = key.ToGuid();
        
        if (!await _routingRepository.Exists(id))
        {
            return ProxyManagerResult.NotFound;
        }

        if (await _routingRepository.Remove(id))
        {
            return ProxyManagerResult.Error;
        }

        var routes = await _routingRepository.Get();
        var routeConfigs = _mapper.Map<IReadOnlyList<RouteConfig>>(routes);

        return _yarpFacade.Update(routeConfigs)
            ? ProxyManagerResult.Ok
            : ProxyManagerResult.Error;
    }
}