using AutoMapper;
using Gateway.Routing.Endpoints.Models;
using Gateway.Routing.Models;

namespace Gateway.Routing.Maps;

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