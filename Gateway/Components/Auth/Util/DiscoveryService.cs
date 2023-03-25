namespace Gateway.Components.Auth.Util;

public class DiscoveryService : IDiscoveryService
{
    private readonly IHttpClientFactory _httpClientFactory;

    private const string DiscoveryUrl = ".well-known/openid-configuration";

    private DiscoveryDocument? _discoveryDocument;

    public DiscoveryService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task< DiscoveryDocument> LoadDiscoveryDocument(string authority)
    {
        if (_discoveryDocument != null)
        {
            return _discoveryDocument;
        }

        var client = _httpClientFactory.CreateClient("discovery-endpoint");

        var doc = await client.GetFromJsonAsync<DiscoveryDocument>(DiscoveryUrl);

        _discoveryDocument = doc ?? throw new Exception("error loading discovery document");

        return doc;
    }
}