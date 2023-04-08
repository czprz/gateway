namespace Gateway.Auth.Endpoints.Models;

public class Userinfo
{
    public string Id { get; set; } = "";
    public string UserName { get; set; } = "";
    public string[] Roles { get; set; } = Array.Empty<string>();
}