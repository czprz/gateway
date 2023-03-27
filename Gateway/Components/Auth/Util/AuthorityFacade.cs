using Gateway.Config;

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

        // TODO: Add validation on discovery document url. Must not start with / or end with /
        var doc = await client.GetFromJsonAsync<DiscoveryDocument>(_config.AuthorityDiscoveryUrl);
        if (doc == null)
        {
            throw new Exception("Unable to load discovery document.");
        }

        _discoveryDocument.TokenEndpoint = new Uri(doc.TokenEndpoint).LocalPath;
    }

    public async Task<TokenResponse?> GetToken(Dictionary<string, string> payload)
    {
        // TODO: Call discovery document endpoint to get token endpoint with caching to avoid too many requests
        var client = _httpClientFactory.CreateClient("authority_endpoint");
        
        var encodedPayload = new FormUrlEncodedContent(payload);
        
        var response = await client.PostAsync(_discoveryDocument.TokenEndpoint, encodedPayload);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        return await response.Content.ReadFromJsonAsync<TokenResponse>();
    }
}