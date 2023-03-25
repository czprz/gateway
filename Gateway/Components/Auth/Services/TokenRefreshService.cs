using Gateway.Components.Auth.Util;
using Gateway.Config;

namespace Gateway.Components.Auth.Services;

public class TokenRefreshService : ITokenRefreshService
{
    private readonly IConfig _config;
    private readonly IHttpClientFactory _clientFactory;

    public TokenRefreshService(IConfig config, IHttpClientFactory clientFactory)
    {
        _config = config;
        _clientFactory = clientFactory;
    }
    
    public async Task<RefreshResponse?> RefreshAsync(string refreshToken)
    {
        var payload = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "refresh_token", refreshToken },
            { "client_id", _config.ClientId },
            { "client_secret", _config.ClientSecret }
        };

        var httpClient = _clientFactory.CreateClient("token_endpoint");
        var response = await httpClient.PostAsync("protocol/openid-connect/token", new FormUrlEncodedContent(payload));

        if (!response.IsSuccessStatusCode) {
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<RefreshResponse>();

        return result;
    }
}