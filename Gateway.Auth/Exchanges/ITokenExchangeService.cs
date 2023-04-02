using Gateway.Routing.Models;

namespace Gateway.Auth.Exchanges;

public interface ITokenExchangeService
{
    Task<TokenExchangeResponse> ExchangeTokenAsync(string token, RouteConfig? routeConfig);
}