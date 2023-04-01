using Gateway.Components.Routing.Models;

namespace Gateway.Components.Auth.Exchanges;

public interface ITokenExchangeService
{
    Task<TokenExchangeResponse> ExchangeTokenAsync(string token, RouteConfig? routeConfig);
}