using Microsoft.Extensions.Options;

namespace Gateway.Common.Config;

public interface IConfig
{
    public bool AuthFlowEnabled { get; }
    public string AuthFlowKey { get; }
    public int SessionTimeoutInMin { get; }
    
    public TokenExchangeStrategy TokenExchangeStrategy { get; }
    
    public Authority Authority { get; }
    public string? ClientId { get; }
    public string? ClientSecret { get; }
    public string? Scopes { get; }
    
    public StorageType? StorageType { get; }
    public string? StorageConnectionString { get; }
    
    // Routing
    public int CheckIntervalSeconds { get; }
    public int InactiveTimeoutSeconds { get; }
}

public record Authority
{
    public Authority(GatewayConfig gatewayConfig)
    {
        Route = CreateRouteUri(gatewayConfig);

        HasInternalRouting = !string.IsNullOrWhiteSpace(gatewayConfig.AuthorityUrl);
        Address = Route?.AbsoluteUri ?? gatewayConfig.Authority;
        RealAddress = gatewayConfig.Authority;
        DiscoveryUrl = gatewayConfig.AuthorityDiscoveryUrl;
    }

    public bool HasInternalRouting { get; }

    public string Address { get; }

    public string RealAddress { get; }

    public Uri? Route { get; set; }

    public string DiscoveryUrl { get; }
    
    private static Uri? CreateRouteUri(GatewayConfig gatewayConfig)
    {
        if (gatewayConfig.AuthorityUrl == null)
        {
            return null;
        }
        
        var baseUri = new Uri("http://localhost");
        return new Uri(baseUri, gatewayConfig.AuthorityUrl);
    }
}