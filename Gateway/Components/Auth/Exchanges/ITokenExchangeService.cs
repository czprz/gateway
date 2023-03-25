using Gateway.Components.Routing.Services;

namespace Gateway.Components.Auth.Exchanges;

public interface ITokenExchangeService
{
    Task<TokenExchangeResponse?> ExchangeTokenAsync(string token, RouteConfig? routeConfig);
}