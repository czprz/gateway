using AutoMapper;
using Gateway.Routing.Models;

namespace Gateway.Routing.Maps;

public class YarpRouteConfigMaps : Profile
{
    public YarpRouteConfigMaps()
    {
        CreateMap<RouteConfig, global::Yarp.ReverseProxy.Configuration.RouteConfig>()
            .ForMember(x => x.RouteId, opt => opt.MapFrom(o => o.Id))
            .ForMember(x => x.ClusterId, opt => opt.MapFrom((_, _, _, context) => context.Items["ClusterId"]))
            .ForMember(d => d.AuthorizationPolicy, opt => opt.MapFrom((_, _, _, context) => context.Items["AuthorizationPolicy"]))
            .ForMember(x => x.Match, opt => opt.MapFrom((route, _, _, context) => GetRouteMatch(route, context)))
            .ReverseMap();
        CreateMap<HeaderMatch, Yarp.ReverseProxy.Configuration.RouteHeader>()
            .ReverseMap();
        CreateMap<QueryParameterMatch, Yarp.ReverseProxy.Configuration.RouteQueryParameter>()
            .ReverseMap();
    }
    
    private static Yarp.ReverseProxy.Configuration.RouteMatch GetRouteMatch(RouteConfig route, ResolutionContext context)
    {
        return new Yarp.ReverseProxy.Configuration.RouteMatch
        {
            Methods = route.Methods,
            Hosts = route.Hosts,
            Headers = route.Headers
                          ?.Select(context.Mapper.Map<HeaderMatch, Yarp.ReverseProxy.Configuration.RouteHeader>)
                          .ToList().AsReadOnly() ??
                      Array.Empty<Yarp.ReverseProxy.Configuration.RouteHeader>().AsReadOnly(),
            Path = route.Path,
            QueryParameters = route.QueryParameters
                                  ?.Select(context.Mapper
                                      .Map<QueryParameterMatch, Yarp.ReverseProxy.Configuration.RouteQueryParameter>)
                                  .ToList().AsReadOnly() ??
                              Array.Empty<Yarp.ReverseProxy.Configuration.RouteQueryParameter>().AsReadOnly()
        };
    }
}