using Gateway.Config;

namespace Gateway.Components.Auth.Services;

public class ApiTokenService : IApiTokenService
{
    public void InvalidateApiTokens(HttpContext ctx)
    {
        throw new NotImplementedException();
    }

    public Task<string> LookupApiToken(HttpContext ctx, ApiConfig apiConfig, string token)
    {
        throw new NotImplementedException();
    }
}