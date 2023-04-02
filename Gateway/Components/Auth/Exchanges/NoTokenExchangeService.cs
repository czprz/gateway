using Gateway.Routing.Models;

namespace Gateway.Components.Auth.Exchanges;

public class NoTokenExchangeService : ITokenExchangeService
{
    public Task<TokenExchangeResponse> ExchangeTokenAsync(string token, RouteConfig? routeConfig)
    {
        var result = new TokenExchangeResponse
        {
            AccessToken = "",
            ExpiresIn = 0,
            RefreshToken = "",
            TokenType = ""
        };
        
        return Task.FromResult(result)!;
    }
}