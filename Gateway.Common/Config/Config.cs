using Microsoft.Extensions.Options;

namespace Gateway.Common.Config;

public class Config : IConfig
{
    private readonly IOptions<GatewayConfig> _options;

    public Config(IOptions<GatewayConfig> options)
    {
        _options = options;
    }

    public bool AuthFlowEnabled => !string.IsNullOrWhiteSpace(_options.Value.AuthorityDiscoveryUrl);
    public string AuthFlowKey => "authPolicy";
    public int SessionTimeoutInMin => _options.Value.SessionTimeoutInMin ?? 10;

    public TokenExchangeStrategy TokenExchangeStrategy =>
        _options.Value.TokenExchangeStrategy ?? TokenExchangeStrategy.TokenExchange;

    public Authority Authority => new(_options.Value);
    
    public string? ClientId => _options.Value.ClientId;
    public string? ClientSecret => _options.Value.ClientSecret;
    public string? Scopes => _options.Value.Scopes;
    
    public StorageType? StorageType => _options.Value.StorageType;
    public string? StorageConnectionString => _options.Value.StorageConnectionString;

    public int CheckIntervalSeconds => _options.Value.CheckIntervalSeconds ?? 300;
    public int InactiveTimeoutSeconds => _options.Value.InactiveTimeoutSeconds ?? 1800;
}