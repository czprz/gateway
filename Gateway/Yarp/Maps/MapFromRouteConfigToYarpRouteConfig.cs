using AutoMapper;
using Gateway.Config;

namespace Gateway.Yarp.Maps;

public class MapFromRouteConfigToYarpRouteConfig : Profile
{
    public MapFromRouteConfigToYarpRouteConfig()
    {
        CreateMap<RouteConfig, global::Yarp.ReverseProxy.Configuration.RouteConfig>()
            .ForMember(x => x.RouteId, opt => opt.MapFrom(o => Guid.NewGuid()))
            .ForMember(x => x.ClusterId, opt => opt.MapFrom((_, _, _, context) => context.Items["ClusterId"]))
            .ReverseMap();
        CreateMap<HeaderMatch, global::Yarp.ReverseProxy.Configuration.RouteHeader>()
            .ReverseMap();
        CreateMap<QueryParameterMatch, global::Yarp.ReverseProxy.Configuration.RouteQueryParameter>()
            .ReverseMap();
    }
}