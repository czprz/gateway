using Gateway.Config;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Gateway.Components.Auth.Handlers;

public class LogoutHandler : ILogoutHandler
{
    private readonly IConfig _config;

    public LogoutHandler(IConfig config)
    {
        _config = config;
    }
    
    public void Logout(RedirectContext context)
    {
        const string logoutUrl = "http://localhost:8080/realms/master/protocol/openid-connect/logout";
        // TODO: Get logout url from discovery document
        if (string.IsNullOrEmpty(logoutUrl))
        {
            return;
        }
        
        var req = context.Request;
        var gatewayUrl = Uri.EscapeDataString(req.Scheme + "://" + req.Host + req.PathBase);

        var logoutUri = logoutUrl
            .Replace("{authority}", _config.Authority)
            .Replace("{clientId}", _config.ClientId)
            .Replace("{gatewayUrl}", gatewayUrl);

        context.Response.Redirect(logoutUri);
        context.HandleResponse();
    }
}