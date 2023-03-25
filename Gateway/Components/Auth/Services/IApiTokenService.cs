using Gateway.Config;

namespace Gateway.Components.Auth.Services;

public interface IApiTokenService
{
    void InvalidateApiTokens(HttpContext ctx);
    Task<string> LookupApiToken(HttpContext ctx, ApiConfig apiConfig, string token);
}