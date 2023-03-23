using AutoMapper;
using Gateway.Config;
using RouteConfig = Gateway.Config.RouteConfig;

namespace Gateway.Yarp.Maps;

public class MapFromRouteConfigToYarpRouteConfig : Profile
{
    public MapFromRouteConfigToYarpRouteConfig()
    {
        CreateMap<RouteConfig, global::Yarp.ReverseProxy.Configuration.RouteConfig>()
            .ForMember(x => x.RouteId, opt => opt.MapFrom(o => Guid.NewGuid()))
            .ForMember(x => x.ClusterId, opt => opt.MapFrom((_, _, _, context) => context.Items["ClusterId"]))
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