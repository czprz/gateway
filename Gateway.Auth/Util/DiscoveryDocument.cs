using System.Text.Json.Serialization;

namespace Gateway.Auth.Util;

public class DiscoveryDocument
{
    [JsonPropertyName("token_endpoint")] 
    public Uri TokenEndpoint { get; set; }
    [JsonPropertyName("userinfo_endpoint")]
    public Uri UserInfoEndpoint { get; set; }
    [JsonPropertyName("end_session_endpoint")]
    public Uri EndSessionEndpoint { get; set; }
}