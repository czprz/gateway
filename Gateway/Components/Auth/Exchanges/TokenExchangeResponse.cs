namespace Gateway.Components.Auth.Exchanges;

public class TokenExchangeResponse
{
    public string AccessToken { get; set; } = "";
    public string RefreshToken { get; set; } = "";
    public string TokenType { get; set; } = "";
    public int ExpiresIn { get; set; }
}