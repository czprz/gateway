using Gateway.Components.Auth.Exchanges;
using Gateway.Components.Auth.Util;
using Gateway.Routing.Models;

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

    public async Task<string> LookupApiToken(HttpContext ctx, string token, RouteConfig? routeConfig)
    {
        var apiToken = ReadApiToken(ctx);
        if (apiToken != null)
        {
            _logger.LogDebug("Using cached API token.");
            // TODO: Perform individual token refresh
            return apiToken.AccessToken;
        }

        var response = await _tokenExchangeService.ExchangeTokenAsync(token, routeConfig);
        SaveApiToken(ctx, response);

        return response.AccessToken;
    }

    private void SaveApiToken(HttpContext ctx, TokenExchangeResponse? response)
    {
        if (response == null)
        {
            _logger.LogDebug("No API token was returned from the token exchange service.");
            return;
        }
        
        ctx.Session.SetString(SessionKeys.API_ACCESS_TOKEN + ".AccessToken", response.AccessToken);
        ctx.Session.SetString(SessionKeys.API_ACCESS_TOKEN + ".RefreshToken", response.RefreshToken);
        ctx.Session.SetString(SessionKeys.API_ACCESS_TOKEN + ".TokenType", response.TokenType);
        ctx.Session.SetInt32(SessionKeys.API_ACCESS_TOKEN + ".ExpiresIn", response.ExpiresIn);
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