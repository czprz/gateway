using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Gateway.Auth.Handlers;

public interface ILogoutHandler
{
    void Logout(RedirectContext context);
}