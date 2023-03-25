using Gateway.Components.Auth.Util;
using Gateway.Config;
using Microsoft.Extensions.Options;

namespace Gateway.Components.Auth.Services;

public class TokenService : ITokenService
{
    private readonly IApiTokenService _apiTokenService;
    private readonly ITokenRefreshService _tokenRefreshService;
    private readonly IOptions<GatewayConfig> _config;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IApiTokenService apiTokenService, ITokenRefreshService tokenRefreshService,
        IOptions<GatewayConfig> config, ILogger<TokenService> logger)
    {
        _apiTokenService = apiTokenService;
        _tokenRefreshService = tokenRefreshService;
        _config = config;
        _logger = logger;
    }

    public async Task AddToken(HttpContext ctx)
    {
        if (IsExpired(ctx) && HasRefreshToken(ctx))
        {
            _apiTokenService.InvalidateApiTokens(ctx);
            await Refresh(ctx);
        }

        var token = ctx.Session.GetString(SessionKeys.ACCESS_TOKEN);
        var currentUrl = ctx.Request.Path.ToString().ToLower();

        var apiConfig = _config.Value.ApiConfigs.FirstOrDefault(x => currentUrl.StartsWith(x.ApiPath));

        if (!string.IsNullOrEmpty(token) && apiConfig != null)
        {
            var apiToken = await GetApiToken(ctx, token, apiConfig);

            _logger.LogDebug("Adding token for request: {currentUrl} {apiToken}", currentUrl, apiToken);

            ctx.Request.Headers.Add("Authorization", "Bearer " + apiToken);
        }
    }

    private async Task<string> GetApiToken(HttpContext ctx, string token, ApiConfig? apiConfig)
    {
        string? apiToken = null;
        if (!string.IsNullOrEmpty(apiConfig?.ApiScopes) || !string.IsNullOrEmpty(apiConfig?.ApiAudience))
        {
            apiToken = await _apiTokenService.LookupApiToken(ctx, apiConfig, token);
        }

        return !string.IsNullOrEmpty(apiToken) ? apiToken : token;
    }

    private async Task Refresh(HttpContext ctx)
    {
        var refreshToken = GetRefreshToken(ctx);

        var resp = await _tokenRefreshService.RefreshAsync(refreshToken);

        if (resp == null)
        {
            // Next call to API will fail with 401 and client can take action
            return;
        }

        var expiresAt = new DateTimeOffset(DateTime.Now).AddSeconds(Convert.ToInt32(resp.expires));

        ctx.Session.SetString(SessionKeys.ACCESS_TOKEN, resp.access_token);
        ctx.Session.SetString(SessionKeys.ID_TOKEN, resp.id_token);
        ctx.Session.SetString(SessionKeys.REFRESH_TOKEN, resp.refresh_token);
        ctx.Session.SetString(SessionKeys.EXPIRES_AT, "" + expiresAt.ToUnixTimeSeconds());
    }

    private static bool HasRefreshToken(HttpContext ctx)
    {
        var refreshToken = ctx.Session.GetString(SessionKeys.REFRESH_TOKEN);
        return !string.IsNullOrEmpty(refreshToken);
    }

    private static string GetRefreshToken(HttpContext ctx)
    {
        var refreshToken = ctx.Session.GetString(SessionKeys.REFRESH_TOKEN);
        return refreshToken ?? "";
    }

    private static bool IsExpired(HttpContext ctx)
    {
        var expiresAt = Convert.ToInt64(ctx.Session.GetString(SessionKeys.EXPIRES_AT)) - 30;
        var now = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();

        var expired = now >= expiresAt;
        return expired;
    }
}