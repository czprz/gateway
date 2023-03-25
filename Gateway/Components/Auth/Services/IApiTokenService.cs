namespace Gateway.Components.Auth.Services;

public interface IApiTokenService
{
    void InvalidateApiTokens(HttpContext ctx);
    Task<string> LookupApiToken(HttpContext ctx, string token);
}