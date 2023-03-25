namespace Gateway.Components.Auth.Util;

public interface IDiscoveryService
{
    Task<DiscoveryDocument> LoadDiscoveryDocument(string authority);
}