using Gateway.Auth.Util.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Gateway.Auth.Util;

public interface IAuthorityFacade
{
    Task<TokenResponse?> GetToken(Dictionary<string, string> payload);
    void Logout(RedirectContext context);
    Task<UserInfo?> GetUserInfo(string accessToken);
}