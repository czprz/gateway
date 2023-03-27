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

    private static async Task<IResult> UseUserInfoEndpoint(ClaimsPrincipal user, IMapper mapper, IHttpClientFactory clientFactory, HttpContext ctx)
    {
        // TODO: Get userinfo from API http://localhost:8080/realms/master/
        var client = clientFactory.CreateClient("authority_endpoint");
        
        var payload = new Dictionary<string, string>
        {
            ["access_token"] = ctx.Session.GetString(SessionKeys.ACCESS_TOKEN)
        };
        
        var response = await client.PostAsync("protocol/openid-connect/userinfo", new FormUrlEncodedContent(payload));
        var content = await response.Content.ReadAsStringAsync();
        var claims = user.Claims;
        // var userInfo = mapper.Map<UserInfo>(claims);
        var userInfo = "";

        return Results.Ok(userInfo);
    }
}