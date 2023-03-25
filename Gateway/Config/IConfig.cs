namespace Gateway.Config;

public interface IConfig
{
    public bool AuthFlowEnabled { get; }
    public string AuthFlowKey { get; }
    public int SessionTimeoutInMin { get; }
    public string TokenExchangeStrategy { get; }
    public string Authority { get; }
    public string ClientId { get; }
    public string ClientSecret { get; }
    public string Scopes { get; }
    public bool QueryUserInfoEndpoint { get; }
}