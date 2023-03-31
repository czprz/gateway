using Asp.Versioning.Builder;
using Gateway.Components.Auth.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Gateway.Components.Auth.Endpoints;

public static class AuthEndpoints
{
    public static void AddAuthEndpoints(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGet("/login", UseLoginEndpoint)
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(1);
        app.MapGet("/logout", UseLogoutEndpoint)
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(1);
        app.MapGet("/userinfo", UseUserInfoEndpoint)
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(1);
    }

    private static void UseLoginEndpoint(string? redirectUrl, HttpContext? context)
    {
        if (string.IsNullOrEmpty(redirectUrl))
        {
            redirectUrl = "/";
        }

        context?.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
        {
            RedirectUri = redirectUrl
        });
    }

    private static IResult UseLogoutEndpoint(string? redirectUrl, HttpContext? context)
    {
        if (string.IsNullOrEmpty(redirectUrl))
        {
            redirectUrl = "/";
        }

        context?.Session.Clear();
        
        var authProps = new AuthenticationProperties
        {
            RedirectUri = redirectUrl
        };
        
        var authSchemes = new[] {
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme
        };

        return Results.SignOut(authProps, authSchemes);
    }

    private static async Task<IResult> UseUserInfoEndpoint(IAuthorityFacade authorityFacade, HttpContext ctx)
    {
        var token = ctx.Session.GetString(SessionKeys.ACCESS_TOKEN);
        if (token == null)
        {
            return Results.Unauthorized();
        }
        
        var userInfo = await authorityFacade.GetUserInfo(token); 
        return Results.Ok(userInfo);
    }
}