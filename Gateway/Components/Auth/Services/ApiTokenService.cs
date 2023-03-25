using Gateway.Components.Auth.Exchanges;
using Gateway.Components.Auth.Util;
using Gateway.Config;

namespace Gateway.Components.Auth.Services;

public class ApiTokenService : IApiTokenService
{
    private readonly ITokenExchangeService _tokenExchangeService;
    private readonly ILogger<ApiTokenService> _logger;

    public ApiTokenService(ITokenExchangeService tokenExchangeService, ILogger<ApiTokenService> logger)
    {
        _tokenExchangeService = tokenExchangeService;
        _logger = logger;
    }

    public void InvalidateApiTokens(HttpContext ctx)
    {
        ctx.Session.Remove(SessionKeys.API_ACCESS_TOKEN);
    }

    public async Task<string> LookupApiToken(HttpContext ctx, string token)
    {
        var apiToken = GetCachedApiToken(ctx);

        if (apiToken != null)
        {
            // TODO: Perform individual token refresh
            return apiToken.AccessToken;
        }

        var response = await _tokenExchangeService.ExchangeTokenAsync(token);
        SetCachedApiToken(ctx, response);

        return response.AccessToken;
    }

    private void SetCachedApiToken(HttpContext ctx, TokenExchangeResponse response)
    {
        SaveApiToken(ctx, response);
    }

    private TokenExchangeResponse? GetCachedApiToken(HttpContext ctx)
    {
        var apiToken = ReadApiToken(ctx);
        return apiToken ?? null;
    }

    private static void SaveApiToken(HttpContext ctx, TokenExchangeResponse token)
    {
        ctx.Session.SetString(SessionKeys.API_ACCESS_TOKEN + ".AccessToken", token.AccessToken);
        ctx.Session.SetString(SessionKeys.API_ACCESS_TOKEN + ".RefreshToken", token.RefreshToken);
        ctx.Session.SetString(SessionKeys.API_ACCESS_TOKEN + ".TokenType", token.TokenType ?? "");
        ctx.Session.SetInt32(SessionKeys.API_ACCESS_TOKEN + ".ExpiresIn", token.ExpiresIn);
    }

    private static TokenExchangeResponse? ReadApiToken(HttpContext ctx)
    {
        if (ctx.Session.GetString(SessionKeys.API_ACCESS_TOKEN + ".TokenType") == null)
        {
            return null;
        }

        return new TokenExchangeResponse
        {
            TokenType = ctx.Session.GetString(SessionKeys.API_ACCESS_TOKEN + ".TokenType")!,
            AccessToken = ctx.Session.GetString(SessionKeys.API_ACCESS_TOKEN + ".AccessToken")!,
            RefreshToken = ctx.Session.GetString(SessionKeys.API_ACCESS_TOKEN + ".RefreshToken")!,
            ExpiresIn = ctx.Session.GetInt32(SessionKeys.API_ACCESS_TOKEN + ".ExpiresIn")!.Value
        };
    }
}