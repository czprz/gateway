using Gateway.Common.Config;
using Gateway.Components.Auth.Endpoints;
using Gateway.Components.Auth.Exchanges;
using Gateway.Components.Auth.Handlers;
using Gateway.Components.Auth.Services;
using Gateway.Components.Auth.Util;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using TokenHandler = Gateway.Components.Auth.Handlers.TokenHandler;

namespace Gateway.Components.Auth;

public static class AuthFlowExtension
{
    public static void AddAuthFlow(this WebApplicationBuilder builder)
    {
        var config = builder.Services.BuildServiceProvider().GetRequiredService<IConfig>();

        builder.Services.AddHttpClient("authority_endpoint", client =>
            {
                client.BaseAddress = new Uri(config.Authority, UriKind.RelativeOrAbsolute);
            });

        // Discovery
        builder.Services.AddSingleton<IAuthorityFacade, AuthorityFacade>();

        // Token Exchange
        builder.Services.AddTokenExchangeService(config);

        // Handlers
        builder.Services.AddTransient<ITokenHandler, TokenHandler>();
        builder.Services.AddTransient<ILogoutHandler, LogoutHandler>();

        // Token Services
        builder.Services.AddTransient<ITokenRefreshService, TokenRefreshService>();
        builder.Services.AddTransient<IApiTokenService, ApiTokenService>();
        builder.Services.AddTransient<ITokenService, TokenService>();

        builder.Services.AddDistributedMemoryCache();

        var sessionTimeoutInMin = config.SessionTimeoutInMin;
        builder.Services.AddSession(opt => { opt.IdleTimeout = TimeSpan.FromMinutes(sessionTimeoutInMin); });

        builder.Services.AddAuthorization(opt =>
        {
            opt.AddPolicy("authPolicy", policy => { policy.RequireAuthenticatedUser(); });
        });

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(setup =>
            {
                setup.ExpireTimeSpan = TimeSpan.FromMinutes(sessionTimeoutInMin);
                setup.SlidingExpiration = true;
            })
            .AddOpenIdConnect(opt =>
            {
                opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.Authority = config.Authority;
                opt.ClientId = config.ClientId;
                opt.UsePkce = true;
                opt.ClientSecret = config.ClientSecret;
                opt.ResponseType = OpenIdConnectResponseType.Code;
                opt.SaveTokens = false;
                opt.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.RequireHttpsMetadata = false;
                
                var scopeArray = config.Scopes?.Split(" ") ?? ArraySegment<string>.Empty;
                foreach (var scope in scopeArray)
                {
                    opt.Scope.Add(scope);
                }

                opt.Events.OnTokenValidated = context =>
                {
                    var tokenHandler = context.HttpContext.RequestServices.GetRequiredService<ITokenHandler>();
                    tokenHandler.HandleToken(context);
                    return Task.CompletedTask;
                };

                opt.Events.OnRedirectToIdentityProviderForSignOut = context =>
                {
                    var logoutHandler = context.HttpContext.RequestServices.GetRequiredService<ILogoutHandler>();
                    logoutHandler.Logout(context);
                    return Task.CompletedTask;
                };
            });
    }

    public static void UseAuthFlow(this WebApplication app)
    {
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCookiePolicy();
        app.AddAuthEndpoints();
    }

    private static void AddTokenExchangeService(this IServiceCollection services, IConfig config)
    {
        switch (config.TokenExchangeStrategy)
        {
            case TokenExchangeStrategy.None:
            {
                services.AddSingleton<ITokenExchangeService, NoTokenExchangeService>();
                break;
            }
            case TokenExchangeStrategy.TokenExchange:
            {
                services.AddTransient<ITokenExchangeService, TokenExchangeService>();
                break;
            }
            default:
            {
                throw new ArgumentOutOfRangeException(nameof(config.TokenExchangeStrategy));
            }
        }
    }
}