using Gateway.Components.Auth.Util;
using Gateway.Components.Routing.Services;

namespace Gateway.Components.Auth.Services;

public interface ITokenRefreshService
{
    Task<RefreshResponse?> RefreshAsync(string refreshToken, RouteConfig? routeConfig);
}