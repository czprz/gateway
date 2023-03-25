using Gateway.Components.Auth.Util;

namespace Gateway.Components.Auth.Services;

public class TokenRefreshService : ITokenRefreshService
{
    public Task<RefreshResponse?> RefreshAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }
}