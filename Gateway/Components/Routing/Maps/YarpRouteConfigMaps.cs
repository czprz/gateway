using AutoMapper;
using Gateway.Components.Routing.Models;

namespace Gateway.Components.Routing.Maps;

public class YarpRouteConfigMaps : Profile
{
    public YarpRouteConfigMaps()
    {
        CreateMap<RouteConfig, global::Yarp.ReverseProxy.Configuration.RouteConfig>()
            .ForMember(x => x.RouteId, opt => opt.MapFrom(o => o.Id))
            .ForMember(x => x.ClusterId, opt => opt.MapFrom((_, _, _, context) => context.Items["ClusterId"]))
            .ForMember(d => d.AuthorizationPolicy, opt => opt.MapFrom((_, _, _, context) => context.Items["AuthorizationPolicy"]))
            .ForMember(x => x.Match, opt => opt.MapFrom((s, _, _, context) =>
            {
                return new global::Yarp.ReverseProxy.Configuration.RouteMatch
                {
                    Methods = s.Methods,
                    Hosts = s.Hosts,
                    Headers =
                        s.Headers?.Select(context.Mapper
                                .Map<HeaderMatch, global::Yarp.ReverseProxy.Configuration.RouteHeader>).ToList()
                            .AsReadOnly() ??
                        Array.Empty<global::Yarp.ReverseProxy.Configuration.RouteHeader>().AsReadOnly(),
                    Path = s.Path,
                    QueryParameters = s.QueryParameters?.Select(
                                              context.Mapper
                                                  .Map<QueryParameterMatch,
                                                      global::Yarp.ReverseProxy.Configuration.RouteQueryParameter>)
                                          .ToList().AsReadOnly() ??
                                      Array.Empty<global::Yarp.ReverseProxy.Configuration.RouteQueryParameter>()
                                          .AsReadOnly()
                };
            }))
            .ReverseMap();
        CreateMap<HeaderMatch, global::Yarp.ReverseProxy.Configuration.RouteHeader>()
            .ReverseMap();
        CreateMap<QueryParameterMatch, global::Yarp.ReverseProxy.Configuration.RouteQueryParameter>()
            .ReverseMap();
    }
}