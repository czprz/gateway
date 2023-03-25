using Gateway.Components.Auth.Services;

namespace Gateway.Components.Auth;

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