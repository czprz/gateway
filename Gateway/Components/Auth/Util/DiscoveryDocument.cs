using System.Text.Json.Serialization;

namespace Gateway.Components.Auth.Util;

public class DiscoveryDocument
{
    [JsonPropertyName("token_endpoint")] 
    public string TokenEndpoint { get; set; } = "";
    [JsonPropertyName("userinfo_endpoint")]
    public string UserInfoEndpoint { get; set; } = "";
    [JsonPropertyName("end_session_endpoint")]
    public string EndSessionEndpoint { get; set; } = "";
}