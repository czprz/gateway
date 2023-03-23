using AutoMapper;
using RouteConfig = Gateway.Config.RouteConfig;

namespace Gateway.Yarp.Maps;

public class MapFromRouteConfigToYarpClusterConfig : Profile
{
    public MapFromRouteConfigToYarpClusterConfig()
    {
        CreateMap<RouteConfig, global::Yarp.ReverseProxy.Configuration.ClusterConfig>()
            .ForMember(d => d.ClusterId, opt => opt.MapFrom(o => Guid.NewGuid()))
            .ForMember(d => d.Destinations, opt => opt.MapFrom(s => s.Proxy.Select(x =>
                new global::Yarp.ReverseProxy.Configuration.DestinationConfig
                {
                    Address = x.Address,
                    Health = x.HealthProbeAddress
                }).ToDictionary(k => k.Address).AsReadOnly()));
    }
}