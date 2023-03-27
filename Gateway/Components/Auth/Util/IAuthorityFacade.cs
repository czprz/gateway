namespace Gateway.Components.Auth.Util;

public interface IAuthorityFacade
{
    Task<TokenResponse?> GetToken(Dictionary<string, string> payload);
}