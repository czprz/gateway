using Gateway.Auth.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway.Auth;

public static class AuthFlowTokenPipe
{
    public static void AddAuthFlowTokenPipe(this IReverseProxyApplicationBuilder pipe)
    {
        var tokenService = pipe.ApplicationServices.GetRequiredService<ITokenService>();

        pipe.Use(async (ctx, next) =>
        {
            await tokenService.AddToken(ctx);
            await next();
        });
    }
}