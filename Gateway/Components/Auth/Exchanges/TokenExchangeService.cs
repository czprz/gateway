using Gateway.Components.Routing.Services;
using Gateway.Config;

namespace Gateway.Components.Auth.Exchanges;

public class TokenExchangeService : ITokenExchangeService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfig _config;

    public TokenExchangeService(IHttpClientFactory httpClientFactory, IConfig config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    public async Task<TokenExchangeResponse?> ExchangeTokenAsync(string token, RouteConfig? routeConfig)
    {
        var httpClient = _httpClientFactory.CreateClient("token_endpoint");
        var payload = new Dictionary<string, string>
        {
            ["grant_type"] = "urn:ietf:params:oauth:grant-type:token-exchange",
            ["client_id"] = routeConfig?.ClientId ?? _config.ClientId,
            ["client_secret"] = routeConfig?.ClientSecret ?? _config.ClientSecret,
            ["audience"] = routeConfig?.Audience ?? "",
            ["scope"] = routeConfig?.Scopes ?? _config.Scopes,
            ["subject_token"] = token,
            ["requested_token_type"] = "urn:ietf:params:oauth:token-type:refresh_token"
        };
        
        // TODO: Attempt to enable token exchange in keycloak
        
        var httpResponse = await httpClient.PostAsync("master/protocol/openid-connect/token", new FormUrlEncodedContent(payload));
        var response = await httpResponse.Content.ReadFromJsonAsync<TokenExchangeResponse>();

        if (response == null)
        {
            throw new Exception("Could not fetch token from exchange.");
        }

        return response;
    }
}