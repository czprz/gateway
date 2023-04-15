using Gateway.Auth.Util;
using Gateway.Common.Extensions;
using Gateway.Routing.Models;

namespace Gateway.Auth.Exchanges;

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
        
        payload.AddIfNotEmpty("client_secret", routeConfig?.ClientSecret);
        payload.AddIfNotEmpty("client_id", routeConfig?.ClientId);
        payload.AddIfNotEmpty("audience", routeConfig?.Audience);
        payload.AddIfNotEmpty("scope", routeConfig?.Scopes);

        var result = await _authorityFacade.GetToken(payload);
        
        return new TokenExchangeResponse
        {
            AccessToken = result?.AccessToken ?? "",
            RefreshToken = result?.RefreshToken ?? "",
            ExpiresIn = result?.Expires ?? 0,
            TokenType = result?.TokenType ?? ""
        };
    }
}