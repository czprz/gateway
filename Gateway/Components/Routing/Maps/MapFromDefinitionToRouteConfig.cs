using AutoMapper;
using Gateway.Components.Routing.Models;
using Gateway.Components.Routing.Services;

namespace Gateway.Components.Routing.Maps;

public class MapFromDefinitionToRouteConfig : Profile
{
    public MapFromDefinitionToRouteConfig()
    {
        CreateMap<RouteDto, RouteConfig>()
            .ReverseMap();
        CreateMap<QueryParameterDto, QueryParameterMatch>()
            .ReverseMap();
        CreateMap<HeaderDto, HeaderMatch>()
            .ReverseMap();
        CreateMap<ProxyDto, Proxy>()
            .ReverseMap();
    }
}