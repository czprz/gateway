namespace Gateway.Components.Auth.Exchanges;

public interface ITokenExchangeService
{
    Task<TokenExchangeResponse> ExchangeTokenAsync(string token);
}