using AutoMapper;
using Gateway.Models;

namespace Gateway.Config.Maps;

public class MapFromDefinitionToRouteConfig : Profile
{
    public MapFromDefinitionToRouteConfig()
    {
        CreateMap<RouteDefinition, RouteConfig>()
            .ReverseMap();
        CreateMap<QueryParameterDefinition, QueryParameterMatch>()
            .ReverseMap();
        CreateMap<HeaderDefinition, HeaderMatch>()
            .ReverseMap();
    }
}