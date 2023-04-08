using AutoMapper;
using Gateway.Auth.Util.Models;

namespace Gateway.Auth.Maps;

public class UserInfoMaps : Profile
{
    public UserInfoMaps()
    {
        CreateMap<UserInfoResponse, UserInfo>()
            .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.RealmAccess.Roles))
            .ReverseMap();
    }
}