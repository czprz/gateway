using Gateway.Config;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Gateway.Components.Auth.Util;

public class AuthorityFacade : IAuthorityFacade
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfig _config;

    private readonly DiscoveryDocument _discoveryDocument = new();

    public AuthorityFacade(IHttpClientFactory httpClientFactory, IConfig config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;

        LoadDiscoveryDocument();
    }

    private async void LoadDiscoveryDocument()
    {
        var client = _httpClientFactory.CreateClient("authority_endpoint");
        
        var doc = await client.GetFromJsonAsync<DiscoveryDocument>(_config.AuthorityDiscoveryUrl);
        if (doc == null)
        {
            throw new Exception("Unable to load discovery document.");
        }
    }

    public async Task<TokenResponse?> GetToken(Dictionary<string, string> payload)
    {
        var client = _httpClientFactory.CreateClient("authority_endpoint");
        
        var encodedPayload = new FormUrlEncodedContent(payload);
        
        var tokenEndpoint = _discoveryDocument.TokenEndpoint.LocalPath;
        var response = await client.PostAsync(tokenEndpoint, encodedPayload);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        return await response.Content.ReadFromJsonAsync<TokenResponse>();
    }

    public void Logout(RedirectContext context)
    {
        var logoutUrl = _discoveryDocument.EndSessionEndpoint.AbsoluteUri;
        context.Response.Redirect(logoutUrl);
        context.HandleResponse();
    }

    public async Task<UserInfoResponse?> GetUserInfo(string accessToken)
    {
        var client = _httpClientFactory.CreateClient("authority_endpoint");
        
        var payload = new Dictionary<string, string>
        {
            ["access_token"] = accessToken
        };
        
        var userInfoEndpoint = _discoveryDocument.UserInfoEndpoint.LocalPath;
        var response = await client.PostAsync(userInfoEndpoint, new FormUrlEncodedContent(payload));
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        return await response.Content.ReadFromJsonAsync<UserInfoResponse>();
    }
}