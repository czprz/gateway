using AutoMapper;
using Yarp.Models;

namespace Yarp.Config.Mapping;

public class MapFromDefinitionToRouteConfig : Profile
{
    public MapFromDefinitionToRouteConfig()
    {
        CreateMap<RouteDefinition, RouteConfig>()
            .ReverseMap();
    }
}