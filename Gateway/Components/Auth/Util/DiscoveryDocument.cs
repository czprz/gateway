using System.Text.Json.Serialization;

namespace Gateway.Components.Auth.Util;

public class DiscoveryDocument
{
    [JsonPropertyName("token_endpoint")] 
    public string TokenEndpoint { get; set; } = "";
}