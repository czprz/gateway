namespace Gateway.Auth;

public class UserInfo
{
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public string[] Roles { get; set; } = Array.Empty<string>();
}