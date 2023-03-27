using Microsoft.Extensions.Options;

namespace Gateway.Config;

public class Config : IConfig
{
    private readonly IOptions<GatewayConfig> _options;

    public Config(IOptions<GatewayConfig> options)
    {
        _options = options;
    }
    
    public bool AuthFlowEnabled => !string.IsNullOrWhiteSpace(_options.Value.AuthorityDiscoveryUrl);
    public string AuthFlowKey => "authPolicy";
    public int SessionTimeoutInMin => _options.Value.SessionTimeoutInMin;
    public string TokenExchangeStrategy => _options.Value.TokenExchangeStrategy ?? "";
    public string Authority => _options.Value.Authority;
    public string AuthorityDiscoveryUrl => _options.Value.AuthorityDiscoveryUrl;
    public string ClientId => _options.Value.ClientId;
    public string ClientSecret => _options.Value.ClientSecret;
    public string Scopes => _options.Value.Scopes;
    public bool QueryUserInfoEndpoint => _options.Value.QueryUserInfoEndpoint;
}