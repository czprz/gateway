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
            if (context.User.Identity?.IsAuthenticated == true && token == null)
            {
                // TODO: Add logic to avoid need for second refresh before token is fetched (only on service restart)
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