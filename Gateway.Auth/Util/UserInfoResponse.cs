namespace Gateway.Auth.Util;

public class UserInfoResponse
{
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string[] Roles { get; set; } = Array.Empty<string>();
    public string Id { get; set; } = "";
}