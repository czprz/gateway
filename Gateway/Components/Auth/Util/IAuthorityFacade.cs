using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Gateway.Components.Auth.Util;

public interface IAuthorityFacade
{
    Task<TokenResponse?> GetToken(Dictionary<string, string> payload);
    void Logout(RedirectContext context);
    Task<UserInfoResponse?> GetUserInfo(string accessToken);
}