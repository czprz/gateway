using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Gateway.Components.Auth.Handlers;

public interface ILogoutHandler
{
    void Logout(RedirectContext context);
}