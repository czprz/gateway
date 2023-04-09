using Gateway.Auth.Util;
using Gateway.Routing.Models;
using Gateway.Routing.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Yarp.ReverseProxy.Model;

namespace Gateway.Auth.Services;

public class TokenService : ITokenService
{
    private readonly IApiTokenService _apiTokenService;
    private readonly ITokenRefreshService _tokenRefreshService;
    private readonly IRoutingRepository _routingRepository;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IApiTokenService apiTokenService, ITokenRefreshService tokenRefreshService,
        IRoutingRepository routingRepository, ILogger<TokenService> logger)
    {
        _apiTokenService = apiTokenService;
        _tokenRefreshService = tokenRefreshService;
        _routingRepository = routingRepository;
        _logger = logger;
    }

    public async Task AddToken(HttpContext ctx)
    {
        var proxy = ctx.Features.Get<IReverseProxyFeature>();
        if (proxy == null)
        {
            return;
        }

        // TODO: CHeck if this is hit when using non-registered routes
        var routeConfig = await _routingRepository.Get(proxy.Route.Config.RouteId);
        if (routeConfig == null)
        {
            return;
        }

        if (IsExpired(ctx) && HasRefreshToken(ctx))
        {
            _apiTokenService.InvalidateApiTokens(ctx);
            await Refresh(ctx, routeConfig);
        }

        var token = ctx.Session.GetString(SessionKeys.ACCESS_TOKEN);
        if (string.IsNullOrEmpty(token) ||
            routeConfig.UseAuthentication == null ||
            !routeConfig.UseAuthentication.Value)
        {
            return;
        }

        var apiToken = await GetApiToken(ctx, token, routeConfig);

        var currentUrl = ctx.Request.Path.ToString().ToLower();
        _logger.LogDebug("Adding token to request: {currentUrl}", currentUrl);

        ctx.Request.Headers.Add("Authorization", "Bearer " + apiToken);
    }

    private async Task<string> GetApiToken(HttpContext ctx, string token, RouteConfig? routeConfig)
    {
        var apiToken = await _apiTokenService.LookupApiToken(ctx, token, routeConfig);

        return !string.IsNullOrEmpty(apiToken) ? apiToken : token;
    }

    private async Task Refresh(HttpContext ctx, RouteConfig? routeConfig)
    {
        var refreshToken = GetRefreshToken(ctx);

        var refreshResponse = await _tokenRefreshService.RefreshAsync(refreshToken, routeConfig);
        if (refreshResponse == null)
        {
            // Next call to API will fail with 401 and client can take action
            return;
        }

        var expiresAt = new DateTimeOffset(DateTime.Now).AddSeconds(Convert.ToInt32(refreshResponse.Expires));

        ctx.Session.SetString(SessionKeys.ACCESS_TOKEN, refreshResponse.AccessToken);
        ctx.Session.SetString(SessionKeys.ID_TOKEN, refreshResponse.IdToken);
        ctx.Session.SetString(SessionKeys.REFRESH_TOKEN, refreshResponse.RefreshToken);
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