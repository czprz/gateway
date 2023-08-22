using AutoMapper;
using Gateway.Routing.Models;
using Gateway.Routing.Storage.Rational.Models;

namespace Gateway.Routing.Storage.Rational.Maps;

public class RouteConfigDbMaps : Profile
{
    public RouteConfigDbMaps()
    {
        CreateMap<RouteConfig, RouteConfigDb>()
            .ForMember(d => d.Id, opt => opt.MapFrom(o => Guid.Parse(o.Id)))
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(o => DateTime.Now))
            .ForMember(d => d.MatchHashCode, opt => opt.MapFrom(o => o.MatchHashCode ?? o.GetMatchHash()))
            .ReverseMap();

        // Methods mapping
        CreateMap<string, MethodDb>()
            .ForMember(d => d.Method, opt => opt.MapFrom(o => o));
        CreateMap<MethodDb, string>()
            .ConvertUsing(d => d.Method);

        // Hosts mapping
        CreateMap<string, HostDb>()
            .ForMember(d => d.Host, opt => opt.MapFrom(o => o));
        CreateMap<HostDb, string>()
            .ConvertUsing(d => d.Host);

        // Upstreams mapping
        CreateMap<Upstream, UpstreamDb>()
            .ReverseMap();

        // Query parameter match values mapping
        CreateMap<QueryParameterMatch, QueryParameterMatchDb>()
            .ForMember(d => d.Values,
                opt => opt.MapFrom(o =>
                    o.Values == null ? null : o.Values.Select(v => new QueryParameterMatchValueDb { Value = v })));
        CreateMap<QueryParameterMatchDb, QueryParameterMatch>()
            .ForMember(d => d.Values,
                opt => opt.MapFrom(o =>
                    o.Values == null ? null : o.Values.Select(v => v.Value)));

        // Header match values mapping
        CreateMap<HeaderMatch, HeaderMatchDb>()
            .ForMember(d => d.Values,
                opt => opt.MapFrom(o =>
                    o.Values == null ? null : o.Values.Select(v => new HeaderMatchValueDb { Value = v })));
        CreateMap<HeaderMatchDb, HeaderMatch>()
            .ForMember(d => d.Values,
                opt => opt.MapFrom(o =>
                    o.Values == null ? null : o.Values.Select(v => v.Value)));

        // Transform mapping
        CreateMap<Transforms, ICollection<TransformDb>>()
            .ConvertUsing(s => s.Map());
        CreateMap<ICollection<TransformDb>, Transforms>()
            .ConvertUsing(s => s.Map());
    }
}