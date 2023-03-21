using AutoMapper;

namespace Yarp.Config.Mapping;

public class MapFromRouteConfigToYarpRouteConfig : Profile
{
    public MapFromRouteConfigToYarpRouteConfig()
    {
        CreateMap<RouteConfig, ReverseProxy.Configuration.RouteConfig>()
            .ReverseMap();
    }
}