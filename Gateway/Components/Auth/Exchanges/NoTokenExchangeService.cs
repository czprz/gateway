using Gateway.Components.Routing.Services;

namespace Gateway.Components.Auth.Exchanges;

public class NoTokenExchangeService : ITokenExchangeService
{
    public Task<TokenExchangeResponse?> ExchangeTokenAsync(string token, RouteConfig? routeConfig)
    {
        var result = new TokenExchangeResponse
        {
            AccessToken = "",
            ExpiresIn = 0,
            RefreshToken = ""
        };
        
        return Task.FromResult(result)!;
    }
}