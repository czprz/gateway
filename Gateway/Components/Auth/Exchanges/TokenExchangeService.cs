using Gateway.Components.Auth.Util;
using Gateway.Routing.Models;

namespace Gateway.Components.Auth.Exchanges;

public class TokenExchangeService : ITokenExchangeService
{
    private readonly IAuthorityFacade _authorityFacade;

    public TokenExchangeService(IAuthorityFacade authorityFacade)
    {
        _authorityFacade = authorityFacade;
    }

    public async Task<TokenExchangeResponse> ExchangeTokenAsync(string token, RouteConfig? routeConfig)
    {
        var payload = new Dictionary<string, string>
        {
            ["grant_type"] = "urn:ietf:params:oauth:grant-type:token-exchange",
            ["subject_token"] = token,
            ["requested_token_type"] = "urn:ietf:params:oauth:token-type:refresh_token"
        };
        
        AddIfNotNull(payload, "client_secret", routeConfig?.ClientSecret);
        AddIfNotNull(payload, "client_id", routeConfig?.ClientId);
        AddIfNotNull(payload, "audience", routeConfig?.Audience);
        AddIfNotNull(payload, "scope", routeConfig?.Scopes);

        var result = await _authorityFacade.GetToken(payload);

        // TODO: Add auto mapping
        return new TokenExchangeResponse
        {
            AccessToken = result?.AccessToken ?? "",
            RefreshToken = result?.RefreshToken ?? "",
            ExpiresIn = result?.Expires ?? 0,
            TokenType = result?.TokenType ?? ""
        };
    }
    
    private static void AddIfNotNull(IDictionary<string, string> dict, string key, string? value)
    {
        if (value != null)
        {
            dict[key] = value;
        }
    }
}