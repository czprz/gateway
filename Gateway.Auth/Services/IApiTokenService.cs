using Gateway.Routing.Models;
using Microsoft.AspNetCore.Http;

namespace Gateway.Auth.Services;

public interface IApiTokenService
{
    void InvalidateApiTokens(HttpContext ctx);
    Task<string> LookupApiToken(HttpContext ctx, string token, RouteConfig? routeConfig);
}