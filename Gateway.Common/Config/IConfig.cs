namespace Gateway.Common.Config;

public interface IConfig
{
    public bool AuthFlowEnabled { get; }
    public string AuthFlowKey { get; }
    public int SessionTimeoutInMin { get; }
    public TokenExchangeStrategy TokenExchangeStrategy { get; }
    public string Authority { get; }
    public string AuthorityDiscoveryUrl { get; }
    public string? ClientId { get; }
    public string? ClientSecret { get; }
    public string? Scopes { get; }
    public StorageType? StorageType { get; }
    public string? StorageConnectionString { get; }
    
    // Routing
    public int CheckIntervalSeconds { get; }
    public int InactiveTimeoutSeconds { get; }
}