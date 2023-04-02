using Gateway.Auth.Util;
using Gateway.Common.Config;
using Gateway.Routing.Models;

namespace Gateway.Auth.Services;

public class TokenRefreshService : ITokenRefreshService
{
    private readonly IConfig _config;
    private readonly IAuthorityFacade _authorityFacade;

    public TokenRefreshService(IConfig config, IAuthorityFacade authorityFacade)
    {
        _config = config;
        _authorityFacade = authorityFacade;
    }

    public async Task<TokenResponse?> RefreshAsync(string refreshToken, RouteConfig? routeConfig)
    {
        var payload = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "refresh_token", refreshToken }
        };
        
        AddIfNotNull(payload, "client_id", routeConfig?.ClientId ?? _config.ClientId);
        AddIfNotNull(payload, "client_secret", routeConfig?.ClientSecret ?? _config.ClientSecret);
        
        var result = await _authorityFacade.GetToken(payload);

        return result;
    }

    private static void AddIfNotNull(IDictionary<string, string> payload, string key, string? value)
    { 
        if (!payload.ContainsKey(key) && !string.IsNullOrEmpty(value))
        {
            payload.Add(key, value);
        }
    }
}