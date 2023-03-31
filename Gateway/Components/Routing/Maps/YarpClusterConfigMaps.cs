using AutoMapper;
using Gateway.Components.Routing.Services;

namespace Gateway.Components.Routing.Maps;

public class YarpClusterConfigMaps : Profile
{
    public YarpClusterConfigMaps()
    {
        CreateMap<RouteConfig, global::Yarp.ReverseProxy.Configuration.ClusterConfig>()
            .ForMember(d => d.ClusterId, opt => opt.MapFrom(o => Guid.NewGuid()))
            .ForMember(d => d.Destinations, opt => opt.MapFrom(s => s.Upstreams.Select(x =>
                // TODO: Add StringComparer.OrdinalIgnoreCase to the dictionary
                new global::Yarp.ReverseProxy.Configuration.DestinationConfig
                {
                    Address = x.Address,
                    Health = x.HealthProbeAddress
                }).ToDictionary(k => k.Address).AsReadOnly()));
    }
}