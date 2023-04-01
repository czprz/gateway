using AutoMapper;
using Gateway.Components.Routing.Models;

namespace Gateway.Components.Routing.Maps;

public class YarpClusterConfigMaps : Profile
{
    public YarpClusterConfigMaps()
    {
        CreateMap<RouteConfig, Yarp.ReverseProxy.Configuration.ClusterConfig>()
            .ForMember(d => d.ClusterId, opt => opt.MapFrom(o => Guid.NewGuid()))
            .ForMember(d => d.LoadBalancingPolicy, opt => opt.MapFrom(o => GetLoadBalancingPolicy(o.LoadBalancingPolicy)))
            .ForMember(d => d.Destinations, opt => opt.MapFrom(s => GetDestinations(s.Upstreams)));
    }
    
    private static IReadOnlyDictionary<string, Yarp.ReverseProxy.Configuration.DestinationConfig> GetDestinations(IEnumerable<Upstream> upstreams)
    {
        var destinations = new Dictionary<string, Yarp.ReverseProxy.Configuration.DestinationConfig>(StringComparer.OrdinalIgnoreCase);
        foreach (var upstream in upstreams)
        {
            destinations.Add(upstream.Address, new Yarp.ReverseProxy.Configuration.DestinationConfig
            {
                Address = upstream.Address,
                Health = upstream.HealthProbeAddress
            });
        }
        return destinations;
    }

    private static string? GetLoadBalancingPolicy(LoadBalancingPolicy? loadBalancingPolicy)
    {
        return loadBalancingPolicy switch
        {
            LoadBalancingPolicy.PowerOfTwoChoices => "PowerOfTwoChoices",
            LoadBalancingPolicy.FirstAlphabetical => "FirstAlphabetical",
            LoadBalancingPolicy.RoundRobin => "RoundRobin",
            LoadBalancingPolicy.LeastRequests => "LeastRequests",
            LoadBalancingPolicy.Random => "Random",
            _ => null
        };
    }
}