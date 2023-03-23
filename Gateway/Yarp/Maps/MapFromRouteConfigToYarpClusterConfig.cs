using AutoMapper;
using Gateway.Config;

namespace Gateway.Yarp.Maps;

public class MapFromRouteConfigToYarpClusterConfig : Profile
{
    public MapFromRouteConfigToYarpClusterConfig()
    {
        CreateMap<RouteConfig, global::Yarp.ReverseProxy.Configuration.ClusterConfig>()
            .ForMember(o => o.ClusterId, opt => opt.MapFrom(o => Guid.NewGuid()))
            .ForMember(o => o.Destinations, opt => opt.MapFrom(o => new global::Yarp.ReverseProxy.Configuration.DestinationConfig
            {
                Address = o.ProxyAddress
            }));
    }
}