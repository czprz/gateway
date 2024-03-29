namespace Gateway.Auth;

public class UserInfo
{
    public string Id { get; set; } = "";
    public string UserName { get; set; } = "";
    public string[] Roles { get; set; } = Array.Empty<string>();
}