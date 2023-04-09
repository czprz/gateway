using Gateway.Auth.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;

namespace Gateway.Auth;

public class TokenCheckMiddleware
{
    private readonly RequestDelegate _next;

    public TokenCheckMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path != "/logout" && context.Request.Path != "/login")
        {
            var token = context.Session.GetString(SessionKeys.ACCESS_TOKEN);
            if (context.Session.IsAvailable && token == null)
            {
                await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
                {
                    RedirectUri = "/"
                });
                
                return;
            }
        }

        await _next(context);
    }
}