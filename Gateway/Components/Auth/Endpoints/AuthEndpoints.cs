using System.Security.Claims;
using AutoMapper;
using Gateway.Components.Auth.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Gateway.Components.Auth.Endpoints;

public static class AuthEndpoints
{
    public static void AddAuthEndpoints(this WebApplication app)
    {
        app.MapGet("/login", UseLoginEndpoint);
        app.MapGet("/logout", UseLogoutEndpoint);
        app.MapGet("/userinfo", UseUserInfoEndpoint);
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
        
        var userInfo = authorityFacade.GetUserInfo(token); 
        return Results.Ok(userInfo);
    }
}