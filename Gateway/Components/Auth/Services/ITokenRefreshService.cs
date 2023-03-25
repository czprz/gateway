using Gateway.Components.Auth.Util;

namespace Gateway.Components.Auth.Services;

public interface ITokenRefreshService
{
    Task<RefreshResponse?> RefreshAsync(string refreshToken);
}