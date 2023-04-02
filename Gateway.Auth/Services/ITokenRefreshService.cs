using Gateway.Auth.Util;
using Gateway.Routing.Models;

namespace Gateway.Auth.Services;

public interface ITokenRefreshService
{
    Task<TokenResponse?> RefreshAsync(string refreshToken, RouteConfig? routeConfig);
}