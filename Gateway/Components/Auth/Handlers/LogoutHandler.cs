using Gateway.Components.Auth.Util;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Gateway.Components.Auth.Handlers;

public class LogoutHandler : ILogoutHandler
{
    private readonly IAuthorityFacade _authorityFacade;

    public LogoutHandler(IAuthorityFacade authorityFacade)
    {
        _authorityFacade = authorityFacade;
    }
    
    public void Logout(RedirectContext context)
    {
        _authorityFacade.Logout(context);
    }
}