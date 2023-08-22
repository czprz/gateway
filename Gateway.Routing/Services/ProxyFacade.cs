using AutoMapper;
using Gateway.Common.Config;
using Gateway.Common.Extensions;
using Gateway.Routing.Models;
using Gateway.Routing.Storage;
using Gateway.Routing.Storage.Rational.Models;

namespace Gateway.Routing.Services;

public class ProxyFacade : IProxyFacade
{
    private readonly IYarpFacade _yarpFacade;
    private readonly IRoutingRepository _routingRepository;
    private readonly IConfig _config;
    private readonly IMapper _mapper;

    public ProxyFacade(IYarpFacade yarpFacade, IRoutingRepository routingRepository, IConfig config, IMapper mapper)
    {
        _yarpFacade = yarpFacade;
        _routingRepository = routingRepository;
        _config = config;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<RouteConfig>> Get()
    {
        var routes = await _routingRepository.Get();

        // TODO: Remove this, instead have it as a migration step
        AddAuthority(routes);

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

    private void AddAuthority(ICollection<RouteConfigDb> routes)
    {
        if (!_config.Authority.HasInternalRouting)
        {
            return;
        }

        // TODO: Add passing of X-Forwarded-For header https://itnext.io/nginx-as-reverse-proxy-in-front-of-keycloak-21e4b3f8ec53
        // TODO: Add passing of X-Forwarded-Proto header
        routes.Add(new RouteConfigDb
        {
            Path = _config.Authority.Route!.LocalPath + "{**catch-all}",
            Transforms = new List<TransformDb>
            {
                new()
                {
                    Values = new List<TransformValuesDb>
                    {
                        new()
                        {
                            Key = "PathRemovePrefix",
                            Value = "/auth"
                        }
                    }
                },
                new()
                {
                    Values = new List<TransformValuesDb>
                    {
                        new()
                        {
                            Key = "X-Forwarded",
                            Value = "proto,for"
                        },
                        new()
                        {
                            Key = "Append",
                            Value = "true"
                        },
                        new()
                        {
                            Key = "Prefix",
                            Value = "X-Forwarded-"
                        }
                    }
                },
                new()
                {
                    Values = new List<TransformValuesDb>
                    {
                        new()
                        {
                            Key = "RequestHeaderOriginalHost",
                            Value = "true"
                        }
                    }
                }
            }.AsReadOnly(),
            Upstreams = new List<UpstreamDb>
            {
                new()
                {
                    Address = _config.Authority.RealAddress
                }
            }
        });
    }
}