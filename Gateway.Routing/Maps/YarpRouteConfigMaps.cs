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
            .ForMember(d => d.Transforms, opt => opt.MapFrom(s => GetTransforms(s)))
            .ReverseMap();
        CreateMap<HeaderMatch, Yarp.ReverseProxy.Configuration.RouteHeader>()
            .ReverseMap();
        CreateMap<QueryParameterMatch, Yarp.ReverseProxy.Configuration.RouteQueryParameter>()
            .ReverseMap();
    }

    private static Yarp.ReverseProxy.Configuration.RouteMatch GetRouteMatch(RouteConfig route,
        ResolutionContext context)
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

    private static IReadOnlyList<IReadOnlyDictionary<string, string>>? GetTransforms(RouteConfig routeConfig)
    {
        if (routeConfig.Transforms == null)
        {
            return null;
        }

        var transforms = new List<IReadOnlyDictionary<string, string>>();

        AddRequestTransforms(transforms, routeConfig);

        return transforms.AsReadOnly();
    }

    private static void AddRequestTransforms(ICollection<IReadOnlyDictionary<string, string>> requestTransforms,
        RouteConfig routeConfig)
    {
        if (routeConfig.Transforms?.RequestTransform == null)
        {
            return;
        }

        var requestTransform = routeConfig.Transforms.RequestTransform;
        var transforms = new Dictionary<string, string>();

        AddTransform(transforms, "PathPrefix", requestTransform.PathPrefix);
        AddTransform(transforms, "PathRemovePrefix", requestTransform.PathRemovePrefix);
        AddTransform(transforms, "PathSet", requestTransform.PathSet);

        requestTransforms.Add(transforms.AsReadOnly());
    }

    private static void AddTransform(IDictionary<string, string> transforms, string key, string? value)
    {
        if (value != null)
        {
            transforms.Add(key, value);
        }
    }
}