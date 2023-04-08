using System.Text.Json.Serialization;

namespace Gateway.Auth.Util.Models;

public class UserInfoResponse
{
    [JsonPropertyName("sub")]
    public string Id { get; set; }
    [JsonPropertyName("preferred_username")]
    public string UserName { get; set; }
    [JsonPropertyName("realm_access")]
    public RealmAccess RealmAccess { get; set; }
}