using AutoMapper;
using Gateway.Routing.Models;
using Gateway.Routing.Storage.Rational.Models;

namespace Gateway.Routing.Storage.Rational.Maps;

public class RouteConfigDbMaps : Profile
{
    public RouteConfigDbMaps()
    {
        CreateMap<RouteConfig, RouteConfigDb>()
            .ForMember(d => d.Id, opt => opt.MapFrom(o => Guid.Parse(o.Id)));
        CreateMap<string, MethodDb>()
            .ForMember(d => d.Method, opt => opt.MapFrom(o => o));
        CreateMap<string, HostDb>()
            .ForMember(d => d.Host, opt => opt.MapFrom(o => o));
        CreateMap<Upstream, UpstreamDb>();
        CreateMap<QueryParameterMatch, QueryParameterMatchDb>()
            .ForMember(d => d.Values,
                opt => opt.MapFrom(o =>
                    o.Values == null ? null : o.Values.Select(v => new QueryParameterMatchValueDb { Value = v })));
        CreateMap<HeaderMatch, HeaderMatchDb>()
            .ForMember(d => d.Values,
                opt => opt.MapFrom(o =>
                    o.Values == null ? null : o.Values.Select(v => new HeaderMatchValueDb { Value = v })));
    }
}