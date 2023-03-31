using AutoMapper;
using Gateway.Components.Routing.Models;
using Gateway.Components.Routing.Services;

namespace Gateway.Components.Routing.Maps;

public class RouteConfigMaps : Profile
{
    public RouteConfigMaps()
    {
        CreateMap<RouteConfigDto, RouteConfig>()
            .ForMember(x => x.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ReverseMap();
        CreateMap<QueryParameterDto, QueryParameterMatch>()
            .ReverseMap();
        CreateMap<HeaderDto, HeaderMatch>()
            .ReverseMap();
        CreateMap<UpstreamDto, Upstream>()
            .ReverseMap();
    }
}