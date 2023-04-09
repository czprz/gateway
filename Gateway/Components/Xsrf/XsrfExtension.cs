using Microsoft.AspNetCore.Antiforgery;

namespace Gateway.Components.Xsrf;

public static class XsrfExtension
{
    public static void AddXsrfCookie(this WebApplicationBuilder builder)
    {
        builder.Services.AddAntiforgery(options =>
        {
            options.Cookie.Name = "XSRF-TOKEN";
            options.Cookie.HttpOnly = false;
            options.HeaderName = "X-XSRF-TOKEN";
        });
    }
    
    public static void UseXsrfCookie(this WebApplication app)
    {
        app.UseXsrfCookieCreator();
        app.UseXsrfCookieChecks();
    }

    private static void UseXsrfCookieCreator(this WebApplication app)
    {
        app.Use(async (ctx, next) =>
        {
            var antiforgery = app.Services.GetService<IAntiforgery>();
            if (antiforgery == null)
            {
                throw new Exception("IAntiforgery service expected!");
            }

            var tokens = antiforgery.GetAndStoreTokens(ctx);
            if (tokens.RequestToken == null)
            {
                throw new Exception("token expected!");
            }

            ctx.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                new CookieOptions { HttpOnly = false });

            await next(ctx);
        });
    }

    private static void UseXsrfCookieChecks(this WebApplication app)
    {
        // TODO: Use RoutingRepository

        app.Use(async (ctx, next) =>
        {
            var antiforgery = app.Services.GetRequiredService<IAntiforgery>();

            var currentUrl = ctx.Request.Path.ToString().ToLower();
            if (!await antiforgery.IsRequestValidAsync(ctx))
            {
                ctx.Response.StatusCode = 400;
                await ctx.Response.WriteAsJsonAsync(new
                {
                    Error = "XSRF token validadation failed"
                });
                return;
            }

            await next(ctx);
        });
    }
}